namespace App.Services
{
    interface IRepository
    {
        int AddTask(Task newTask);
        void DeleteTask(int task_id);
        void UpdateTaskTitle(int task_id, string newTitle);
        void UpdateTaskDescription(int task_id, string newDescription);
        void UpdateTaskDueTime(int task_id, DateTimeOffset newDue);
        Task GetTask(int task_id);
        IEnumerable<Task> GetTasks();
    }
}
