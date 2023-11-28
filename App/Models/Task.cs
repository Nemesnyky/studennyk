namespace App.Models;

public class Task
{
    public const long DEFAULT_ID = -1;
    public const bool DONE = true;
    public const bool NOT_DONE = false;

    public long Id { get; set; } = DEFAULT_ID;
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Due { get; set; }
    public bool IsDone { get; set; } = NOT_DONE;

    /// <summary>
    /// <para>Use DEFAULT_ID for Id if it is not known.</para>
    /// <para>Use NOT_DONE for isDone when creating tasks</para>
    /// </summary>
    public Task(long id, string title, string description, DateTimeOffset created, DateTimeOffset due, bool isDone)
    {
        Id = id;
        Title = title;
        Description = description;
        Created = created;
        Due = due;
        IsDone = isDone;
    }
}
