namespace StudennykApi.Models;

public class UpdateTask
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? Created { get; set; }
    public DateTimeOffset? Due { get; set; }
    public bool? IsDone { get; set; }
}
