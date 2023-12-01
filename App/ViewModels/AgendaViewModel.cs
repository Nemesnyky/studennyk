using System.Collections.ObjectModel;
using App.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Task = App.Models.Task;

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
        private TaskListViewModel taskListVM;
        private Task dragged;

        private List<Task> tasks;
        public ObservableCollection<TaskGroup> TaskGroups { get; set; }

        public AgendaViewModel()
        {
            taskListVM = new TaskListViewModel();
            tasks = new List<Task>();
            TaskGroups = new ObservableCollection<TaskGroup>();
            System.Threading.Tasks.Task.Run(async () => { await LoadTasks(); });
        }

        private async System.Threading.Tasks.Task LoadTasks()
        {
            var taskList = await System.Threading.Tasks.Task.Run(() => this.taskListVM.GetTaskList());

            var dates = new List<DateTimeOffset>();

            tasks.Clear();
            TaskGroups.Clear();
            foreach (var task in taskList.Where(t => t.IsDone != true))
            {
                tasks.Add(task);
                if (dates.All(t => t.Hour != task.Due.Hour))
                {
                    dates.Add(task.Due);
                }
            }

            foreach (var date in dates.OrderBy(t => t.Hour))
            {
                TaskGroups.Add(new TaskGroup(date,
                    taskList.Where(t => t.Due.Hour == date.Hour && t.IsDone != true)));
            }
        }

        public async System.Threading.Tasks.Task DeleteTask(int taskId)
        {
            tasks.Remove(tasks.Single(t => t.Id == taskId));
            await System.Threading.Tasks.Task.Run(() => taskListVM.DeleteTask(taskId));
        }


        private void CompleteTask(long taskId)
        {
            var task = tasks.First(t => t.Id == taskId);
            task.IsDone = true;

            var taskGroup = TaskGroups.First(t => t.Date.Hour == task.Due.Hour);

            if (taskGroup.Any(t => t.Id != task.Id))
            {
                taskGroup.Remove(task);
            }
            else
            {
                TaskGroups.Remove(taskGroup);
            }

            System.Threading.Tasks.Task.Run(() => taskListVM.CompleteTask(taskId));
        }

        [RelayCommand]
        private void DragStarted(Task task)
        {
            dragged = task;
        }

        [RelayCommand]
        private void TaskDropped()
        {
            CompleteTask(dragged.Id);
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
    }
}
