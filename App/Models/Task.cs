public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime Due { get; set; }
    public bool IsDone { get; set; }

    public Task(int id, string title, string description, DateTime created, DateTime due, bool isDone)
    {
        Id = id;
        Title = title;
        Description = description;
        Created = created;
        Due = due;
        IsDone = isDone;
    }
}
