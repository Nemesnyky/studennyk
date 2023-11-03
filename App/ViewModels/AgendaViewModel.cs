using System.Collections.ObjectModel;
using App.Models;
using CommunityToolkit.Mvvm.Input;
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
        private TaskController taskController;
        private Task dragged;

        private List<Task> tasks;
        public ObservableCollection<TaskGroup> TaskGroups { get; set; }

        public AgendaViewModel()
        {
            taskController = new TaskController();
            tasks = new List<Task>();
            TaskGroups = new ObservableCollection<TaskGroup>();
            System.Threading.Tasks.Task.Run(async () => { await LoadTasks(0); });
        }

        private async System.Threading.Tasks.Task LoadTasks(int userId)
        {
            var taskList = await System.Threading.Tasks.Task.Run(() => taskController.GetTaskList(userId));

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
            await System.Threading.Tasks.Task.Run(() => taskController.DeleteTask(taskId));
        }


        private void CompleteTask(int taskId)
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

            System.Threading.Tasks.Task.Run(() => taskController.CompleteTask(taskId));
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



    }
}
