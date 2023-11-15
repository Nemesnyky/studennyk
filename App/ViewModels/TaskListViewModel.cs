using App.Repositories;
//using System.Collections.ObjectModel;
using Task = App.Models.Task;
using ThreadTask = System.Threading.Tasks.Task;

namespace App.ViewModels
{

    public class TaskListViewModel
    {
        private readonly IRepository repository = new SQLiteRepository();
        //public ObservableCollection<Task> Tasks { get; set; } = new();

        public TaskListViewModel()
        {
            RepositoryInitialize();
            //LoadTasks();
        }

        private void RepositoryInitialize()
        {
            Random random = new();
            int numTasks = random.Next(5, 12);
            for (int i = 0; i < numTasks; i++)
                repository.AddTask(Temporary.Generator.GenerateRandomTask());
        }
        /*
        public void LoadTasks()
        {
            Tasks.Clear();
            var loadTasks = repository.GetTasks();
            foreach (var task in loadTasks)
                Tasks.Add(task);
        }
        */
        public IEnumerable<Task> GetTaskList()
        {
            return repository.GetTasks();
        }

        public void DeleteTask(long taskId)
        {
            repository.DeleteTask(taskId);
            //Tasks.Remove(Tasks.Single(t => t.Id == taskId));
        }

        public async ThreadTask CompleteTask(long taskId)
        {
            //TODO
            await ThreadTask.Delay(10);
        }
    }

}
