public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Due { get; set; }
    public bool IsDone { get; set; }

    public Task(int id, string title, string description, DateTimeOffset created, DateTimeOffset due, bool isDone)
    {
        Id = id;
        Title = title;
        Description = description;
        Created = created;
        Due = due;
        IsDone = isDone;
    }
    
    public override bool Equals(object obj)
    {
        return obj is Task task &&
            Id == task.Id &&
            Title == task.Title &&
            Description == task.Description &&
            Created.ToString() == task.Created.ToString() &&
            Due.ToString() == task.Due.ToString() &&
            IsDone == task.IsDone;
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}
