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
               Created.Equals(task.Created) &&
               Due.Equals(task.Due) &&
               IsDone == task.IsDone;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Title, Description, Created, Due, IsDone);
    }
}
