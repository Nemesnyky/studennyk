using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
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
        private TaskListViewModel taskListVM;
        private Task dragged;

        private List<Task> tasks;
        public ObservableCollection<TaskGroup> TaskGroups { get; set; }

        public AgendaViewModel()
        {
            taskListVM = new TaskListViewModel();
            tasks = new List<Task>();
            TaskGroups = new ObservableCollection<TaskGroup>();
            ThreadTask.Run(async () => { await LoadTasks(); });
        }

        private async ThreadTask LoadTasks()
        {
            var taskList = await ThreadTask.Run(async () =>
            {
                await taskListVM.LoadTasks();
                return taskListVM.GetTaskList();
            });

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

        public async ThreadTask DeleteTask(int taskId)
        {
            tasks.Remove(tasks.Single(t => t.Id == taskId));
            await ThreadTask.Run(() => taskListVM.DeleteTask(taskId));
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

            ThreadTask.Run(() => taskListVM.CompleteTask(taskId));
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
