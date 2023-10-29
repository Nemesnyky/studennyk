namespace App.Services
{
    interface IRepository
    {
        long? AddTask(Task newTask);
        void DeleteTask(long? task_id);
        void UpdateTaskTitle(long? task_id, string newTitle);
        void UpdateTaskDescription(long? task_id, string newDescription);
        void UpdateTaskDueTime(long? task_id, DateTimeOffset newDue);
        Task GetTask(long? task_id);
        IEnumerable<Task> GetTasks();
    }
}
