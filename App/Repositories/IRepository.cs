using Task = App.Models.Task;

namespace App.Repositories
{
    public interface IRepository
    {
        long AddTask(Task newTask);
        void DeleteTask(long taskId);
        void CompleteTask(long taskId);
        void UpdateTaskTitle(long taskId, string newTitle);
        void UpdateTaskDescription(long taskId, string newDescription);
        void UpdateTaskDueTime(long taskId, DateTimeOffset newDue);
        Task GetTask(long taskId);
        IEnumerable<Task> GetTasks();
    }
}
