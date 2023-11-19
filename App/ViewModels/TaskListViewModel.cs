using App.Repositories;
using App.Services;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Task = App.Models.Task;

namespace App.ViewModels
{

    public class TaskListViewModel
    {
        private readonly IRepository repository;
        private readonly ILogger logger;
        public ObservableCollection<Task> Tasks { get; set; } = new();

        public TaskListViewModel()
        {

            repository = AppServiceProvider.GetService<IRepository>();
            logger = AppLoggerProvider.CreateLogger<TaskListViewModel>();
            LoadTasks();
        }

        public void LoadTasks()
        {
            logger.LogInformation("LoadTask from DataBase");
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
            logger.LogInformation($"DeleteTask (ID = {taskId})");
            repository.DeleteTask(taskId);
            Tasks.Remove(Tasks.Single(t => t.Id == taskId));
        }

        public void CompleteTask(long taskId)
        {
            logger.LogInformation($"CompleteTask (ID = {taskId})");
            repository.UpdateTaskStatus(taskId, Task.DONE);
            int i = Tasks.IndexOf(Tasks.Single(t => t.Id == taskId));
            Tasks[i].IsDone = true;
        }
    }

}
