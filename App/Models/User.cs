public class User
{
    public int Id { get; set; }
    public User(int id)
    { 
        Id = id;
    }
}

public enum Permissions : short
{
    None,
    ReadOnly,
    ReadWrite,
}