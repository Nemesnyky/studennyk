using System.Collections.ObjectModel;
using App.Models;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels
{
    public class TaskGroup : ObservableCollection<TaskModel>
    {
        public DateTimeOffset Date { get; private set; }

        public TaskGroup(DateTimeOffset date, IEnumerable<TaskModel> tasks) : base(tasks)
        {
            Date = date;
        }
    }

    public partial class AgendaViewModel
    {
        private TaskController taskController;
        private TaskModel dragged;

        private List<TaskModel> tasks;
        public ObservableCollection<TaskGroup> TaskGroups { get; set; }

        public AgendaViewModel()
        {
            taskController = new TaskController();
            tasks = new List<TaskModel>();
            TaskGroups = new ObservableCollection<TaskGroup>();
            Task.Run(async () => { await LoadTasks(0); });
        }

        private async Task LoadTasks(int userId)
        {
            var taskList = await Task.Run(() => taskController.GetTaskList(userId));

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

        public async Task DeleteTask(int taskId)
        {
            tasks.Remove(tasks.Single(t => t.Id == taskId));
            await Task.Run(() => taskController.DeleteTask(taskId));
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

            Task.Run(() => taskController.CompleteTask(taskId));
        }

        [RelayCommand]
        private void DragStarted(TaskModel task)
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
