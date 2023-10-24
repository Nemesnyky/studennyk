using DataBaseService;

public static class Constants
{
    public static SQLiteRepository REPOSITORY;
    public static User DEFAULT_USER;
    static Constants()
    {
        DEFAULT_USER = new User(0);
        REPOSITORY = new SQLiteRepository("Data Source=:memory:");
    }
}
