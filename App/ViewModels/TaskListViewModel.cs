using App.Services;
using System.Collections.ObjectModel;

namespace App.ViewModels
{

    public class TaskListViewModel
    {
        private readonly RestService client;
        private List<Models.Task> tasks;

        public TaskListViewModel()
        {
            client = AppServiceProvider.GetService<RestService>();
            Task.Run(async () =>
            {
                await Task.Delay(100);
                await LoadTasks();
            });
        }

        public List<Models.Task> GetTaskList()
        {
            return tasks;
        }

        public async Task LoadTasks()
        {
            tasks = await client.RefreshDataAsync();
        }

        public async Task DeleteTask(long taskId)
        {
            await client.DeleteTask(taskId);
            tasks.Remove(tasks.Single(t => t.Id == taskId));
        }

        public async Task CompleteTask(long taskId)
        {
            await client.UpdateTaskStatus(taskId, Models.Task.DONE);
            int i = tasks.IndexOf(tasks.Single(t => t.Id == taskId));
            tasks[i].IsDone = true;
        }
    }

}
