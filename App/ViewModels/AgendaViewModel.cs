using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using App.Repositories;
using App.Services;
using Task = App.Models.Task;
using ThreadTask = System.Threading.Tasks.Task;

namespace App.ViewModels
{
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
        private Task dragged;
        private readonly IRepository repository;
        private readonly List<Task> allTasks = new();
        public ObservableCollection<TaskGroup> TaskGroups { get; set; } = new();

        public AgendaViewModel()
        {
            repository = AppServiceProvider.GetService<IRepository>();

            ThreadTask.Run(LoadTasks);
        }

        private async ThreadTask LoadTasks()
        {

            TaskGroups.Clear();
            allTasks.Clear();
            allTasks.AddRange(await ThreadTask.Run(repository.GetTasks));

            List<DateTimeOffset> dates = new();
            List<Task> notDoneTasks = new();

            foreach (var task in allTasks.Where(t => t.IsDone == Task.NOT_DONE))
            {
                notDoneTasks.Add(task);
                if (dates.All(t => t.Hour != task.Due.Hour))
                {
                    //TODO : ñonsider days, not just hours
                    dates.Add(task.Due);
                }
            }

            foreach (var date in dates.OrderBy(t => t.Hour))
            {
                //TODO : consider days, not just hours
                TaskGroups.Add(new TaskGroup(
                    date,
                    notDoneTasks.Where(t => t.Due.Hour == date.Hour))
                    );
            }
        }

        public async ThreadTask DeleteTask(int taskId)
        {
            allTasks.Remove(allTasks.Single(t => t.Id == taskId));
            //TODO : delete task from TaskGroups
            await ThreadTask.Run(() => repository.DeleteTask(taskId));
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
    }
}
