namespace App.Services
{
    interface IRepository
    {
        int AddTask(User user, Task newTask);
        void DeleteTask(User user, int task_id);
        void UpdateTaskTitle(User user, int task_id, string newTitle);
        void UpdateTaskDescription(User user, int task_id, string newDescription);
        void UpdateTaskDueTime(User user, int task_id, DateTimeOffset newDue);
        Task GetTask(User user, int task_id);
        IEnumerable<Task> GetTasks(User user);
    }
}
