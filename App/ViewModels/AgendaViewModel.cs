using System.Collections.ObjectModel;
using App.Models;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels
{
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

        private void RegroupTasks()
        {
            TaskGroups.Clear();
            var dates = new List<DateTimeOffset>();
            var notCompleteTasks = tasks.Where(t => t.IsDone != true).ToList();
            foreach (var task in notCompleteTasks)
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
                    tasks.Where(t => t.Due.Hour == date.Hour && t.IsDone != true)));
            }
        }

        private void CompleteTask(int taskId)
        {
            var task = tasks.First(t => t.Id == taskId);
            task.IsDone = true;

            var taskGroup = TaskGroups.First(t => t.Date.Hour == task.Due.Hour);

            if (taskGroup.Count(t => t.Id != task.Id) != 0) {
                var tasks = taskGroup.Where(t => t.Id != task.Id).ToList();
                taskGroup.Clear();
                foreach (var taskModel in tasks)
                {
                    taskGroup.Add(taskModel);
                }
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