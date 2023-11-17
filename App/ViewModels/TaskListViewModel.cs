using App.Repositories;
using App.Services;
using System.Collections.ObjectModel;
using Task = App.Models.Task;
using ThreadTask = System.Threading.Tasks.Task;

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
            foreach (var task in repository.GetTasks())
                Tasks.Add(task);
        }
        
        public List<Task> GetTaskList()
        {
            return Tasks.ToList();
        }

        public void DeleteTask(long task_id)
        {
            repository.DeleteTask(task_id);
            Tasks.Remove(Tasks.Single(t => t.Id == task_id));
        }

        public void CompleteTask(long task_id)
        {
            repository.CompleteTask(task_id);
            int i = Tasks.IndexOf(Tasks.Single(t => t.Id == task_id));
            Tasks[i].IsDone = true;
            //await ThreadTask.Delay(10);
        }
    }

}
