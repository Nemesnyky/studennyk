using App.Repositories;
using App.Services;
using System.Collections.ObjectModel;
using Task = App.Models.Task;

namespace App.ViewModels
{

    public class TaskListViewModel
    {
        private readonly IRepository repository;
        public ObservableCollection<Task> Tasks { get; set; } = new();

        public TaskListViewModel()
        {
            repository = AppServiceProvider.GetService<IRepository>();
            LoadTasks();
        }

        public void LoadTasks()
        {
            Tasks.Clear();
            foreach (Task task in repository.GetTasks())
            {
                Tasks.Add(task);
            }
        }

        public List<Task> GetTaskList()
        {
            return Tasks.ToList();
        }

        public void DeleteTask(long taskId)
        {
            repository.DeleteTask(taskId);
            Tasks.Remove(Tasks.Single(t => t.Id == taskId));
        }

        public void CompleteTask(long taskId)
        {
            repository.UpdateTaskStatus(taskId, Task.DONE);
            int i = Tasks.IndexOf(Tasks.Single(t => t.Id == taskId));
            Tasks[i].IsDone = true;
        }
    }

}
