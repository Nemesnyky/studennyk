using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using App.Repositories;
using App.Services;
using CommunityToolkit.Mvvm.Messaging;
using Task = App.Models.Task;
using ThreadTask = System.Threading.Tasks.Task;
using System.Data.SqlTypes;

namespace App.ViewModels
{
    public sealed class ReloadTasksMessage { }
    public class TaskGroup : ObservableCollection<Task>
    {
        public DateTimeOffset Date { get; private set; }

        public TaskGroup(DateTimeOffset date, IEnumerable<Task> tasks) : base(tasks)
        {
            Date = date;
        }
    }

    public partial class AgendaViewModel
    {
        public IRepository repository;
        private Task dragged;
        private readonly List<Task> allTasks = new();
        public ObservableCollection<TaskGroup> TaskGroups { get; set; } = new();

        public AgendaViewModel()
        {
            repository = AppServiceProvider.GetService<IRepository>();
            allTasks = repository.GetTasks().ToList();
            LoadTaskGroups(allTasks);
            
        }

        public void AddTask(Task task)
        {
            allTasks.Add(task);
            LoadTaskGroups(allTasks);
        }

        private void LoadTaskGroups(IEnumerable<Task> tasks) 
        {
            List<DateTimeOffset> dates = new();
            List<Task> notDoneTasks = new();

            foreach (var task in tasks.Where(t => t.IsDone == Task.NOT_DONE))
            {
                notDoneTasks.Add(task);
                if (dates.All(t => t.Hour != task.Due.Hour))
                {
                    //TODO : ñonsider days, not just hours
                    dates.Add(task.Due);
                }
            }
            var taskGroups = new List<TaskGroup>();
            foreach (var date in dates.OrderBy(t => t.Hour))
            {
                //TODO : consider days, not just hours
                taskGroups.Add(new TaskGroup(
                    date,
                    notDoneTasks.Where(t => t.Due.Hour == date.Hour))
                    );
            }
            int a = 7;
            TaskGroups.Clear();
            foreach (var taskGroup in taskGroups) 
            { 
                TaskGroups.Add(taskGroup);
            }
        }


        public async ThreadTask DeleteTask(int taskId)
        {
            allTasks.Remove(allTasks.Single(t => t.Id == taskId));
            //TODO : delete task from TaskGroups
             repository.DeleteTask(taskId);
        }

        private async ThreadTask CompleteTask(long taskId)
        {
            Task task = allTasks.First(t => t.Id == taskId);
            task.IsDone = Task.DONE;

            var taskGroup = TaskGroups.First(t => t.Date.Hour == task.Due.Hour);

            if (taskGroup.Any(t => t.Id != task.Id))
            {
                taskGroup.Remove(task);
            }
            else
            {
                TaskGroups.Remove(taskGroup);
            }

            await ThreadTask.Run(() => repository.UpdateTaskStatus(taskId, Task.DONE));
        }
       

        [RelayCommand]
        private void DragStarted(Task task)
        {
            dragged = task;
        }

        [RelayCommand]
        private void TaskDropped()
        {
            _ = CompleteTask(dragged.Id);
            dragged = null;
        }
        [RelayCommand]
        public void ShowDescription(Task task)
        {
            WeakReferenceMessenger.Default.Send(new ShowDescriptionMessage(task));
        }
        [RelayCommand]
        public void ShowNewTask()
        {
            WeakReferenceMessenger.Default.Send(new ShowNewTaskMessage());
        }
        public void ReloadTasks() 
        {
           
        }
    }
}
