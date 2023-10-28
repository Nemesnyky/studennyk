using App.Models;

public class TaskController
{
    private Random random = new Random();

    public List<TaskModel> GetTaskList(int userId = 0)
    {
        List<TaskModel> tasks = new List<TaskModel>();

        int numTasks = random.Next(1, 11);

        for (int i = 0; i < numTasks; i++)
        {
            TaskModel task = GenerateRandomTask();
            tasks.Add(task);
        }

        return tasks;
    }

    public bool DeleteTask(int taskId)
    {
        return true;
    }

    private TaskModel GenerateRandomTask()
    {
        int id = random.Next(1000, 10000);
        string[] titles = { "Buy groceries", "Finish project", "Call mom", "Exercise", "Read a book", "Clean the house" };
        string title = titles[random.Next(titles.Length)];

        string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        string description = lorem.Substring(random.Next(0, 200), random.Next(51, lorem.Length - 201));

        DateTimeOffset created = DateTimeOffset.Now.AddDays(random.Next(1, 3)).AddHours(random.Next(1, 12));
        DateTimeOffset due = created.AddDays(random.Next(1, 7));

        return new TaskModel(id, title, description, created, due, false);
    }

    public async Task CompleteTask(int taskId)
    {
        await Task.Delay(10);
    }
}

