namespace StudennykApi.Models;

public class UpdateTask
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Due { get; set; }
    public bool? IsDone { get; set; }
}
