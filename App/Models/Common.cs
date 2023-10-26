using App.Services;

public static class Common
{
    public static SQLiteRepository REPOSITORY;
    public static User DEFAULT_USER;
    static Common()
    {
        DEFAULT_USER = new User(0);
        REPOSITORY = new SQLiteRepository("Data Source=:memory:");
    }
    public static Task GenerateRandomTask()
    {
        Random random = new Random();
        string[] titles = { "Buy groceries", "Finish project", "Call mom", "Exercise", "Read a book", "Clean the house" };
        string lorem = "qwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqazwsxedcrfvtgbyhnujmikolp";
        return new Task(
            random.Next(1, 100000),
            titles[random.Next(titles.Length)],
            lorem.Substring(random.Next(0, lorem.Length / 2), random.Next(1, lorem.Length / 2)),
            DateTimeOffset.Now.AddDays(random.Next(1000)),
            DateTimeOffset.Now.AddDays(random.Next(1000)),
            random.Next(2) == 1
            );
    }
}
