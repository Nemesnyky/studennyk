using System.Collections.ObjectModel;

namespace App.Models
{
    
public class TaskModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Due { get; set; }
    public bool IsDone { get; set; }

    public TaskModel(int id, string title, string description, DateTimeOffset created, DateTimeOffset due, bool isDone)
    {
        Id = id;
        Title = title;
        Description = description;
        Created = created;
        Due = due;
        IsDone = isDone;
    }
}
    public class TaskGroup : ObservableCollection<TaskModel>
    {
        public DateTimeOffset Date { get; private set; }

        public TaskGroup(DateTimeOffset date, IEnumerable<TaskModel> tasks) : base(tasks)
        {
            Date = date;
        }
    }
}
