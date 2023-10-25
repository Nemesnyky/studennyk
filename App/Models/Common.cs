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
        return new Task(
            0,
            random.Next(1, 100000).ToString(),
            random.Next(1, 100000).ToString(),
            DateTimeOffset.Now.AddDays(random.Next(1000)),
            DateTimeOffset.Now.AddDays(random.Next(1000)),
            random.Next(2) == 1
            );
    }
}
