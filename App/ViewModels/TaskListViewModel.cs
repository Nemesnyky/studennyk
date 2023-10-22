using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class TaskListViewModel : INotifyPropertyChanged
{
    private TaskController taskController = new TaskController();
    private ObservableCollection<Task> tasks;

    public ObservableCollection<Task> Tasks
    {
        get { return tasks; }
        set
        {
            tasks = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public TaskListViewModel()
    {
        Tasks = new ObservableCollection<Task>();
    }

    public async void LoadTasks(int userId)
    {
        var taskList = await System.Threading.Tasks.Task.Run(() => taskController.GetTaskList(userId));

        Tasks.Clear();
        foreach (var task in taskList)
        {
            Tasks.Add(task);
        }
    }

    public async void DeleteTask(int taskId)
    {
        Tasks.Remove(tasks.Single(t => t.Id == taskId));
        await System.Threading.Tasks.Task.Run(() => taskController.DeleteTask(taskId));
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
