using Task = App.Models.Task;

namespace App.Temporary
{
    public static class Generators
    {
        private static readonly Random random = new();

        public static Task GenerateRandomTask()
        {
            string[] titles = { "Buy groceries", "Finish project", "Call mom", "Exercise", "Read a book", "Clean the house" };
            string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

            string title = titles[random.Next(titles.Length)];
            string description = lorem.Substring(random.Next(0, 200), random.Next(51, lorem.Length - 201));
            DateTimeOffset created = DateTimeOffset.Now.AddDays(random.Next(1, 3)).AddHours(random.Next(1, 12));
            DateTimeOffset due = created.AddDays(random.Next(1, 7));

            return new Task(Task.DEFAULT_ID, title, description, created, due, Task.NOT_DONE);
        }
    }
}