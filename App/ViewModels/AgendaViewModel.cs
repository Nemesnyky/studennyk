using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using App.Models;
using CommunityToolkit.Mvvm.Input;

namespace App.ViewModels
{
public partial class AgendaViewModel
{
    private TaskController taskController = new TaskController();
    private ObservableCollection<TaskModel> tasks;
    private TaskModel dragged;

    public ObservableCollection<TaskModel> Tasks
    {
        get => tasks;
        set
        {
            tasks = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public AgendaViewModel()
    {
        Tasks = new ObservableCollection<TaskModel>();
        Task.Run(async () =>
        {
            await LoadTasks(0);
        });
    }

    private async Task LoadTasks(int userId)
    {
        var taskList = await Task.Run(() => taskController.GetTaskList(userId));

        Tasks.Clear();
        foreach (var task in taskList.Where(t => t.IsDone != true))
        {
            Tasks.Add(task);
        }
    }

    public async Task DeleteTask(int taskId)
    {
        Tasks.Remove(tasks.Single(t => t.Id == taskId));
        await Task.Run(() => taskController.DeleteTask(taskId));
    }

    private void UpdateView()
    {
        Tasks = new ObservableCollection<TaskModel>(Tasks.Where(t => t.IsDone != true));
    }

    private async Task CompleteTask(int taskId)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == taskId)!;
        task.IsDone = true;
        Tasks.Remove(task);
        await Task.Run(() => taskController.CompleteTask(taskId));
    }
    

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    [RelayCommand]
    private void DragStarted(TaskModel task)
    {
        dragged = task;
    }

    [RelayCommand]
    private async Task TaskDropped()
    {
        await CompleteTask(dragged.Id);
        dragged = null;
    }
}
}
