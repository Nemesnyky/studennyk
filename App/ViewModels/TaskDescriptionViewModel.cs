﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Task = App.Models.Task;

namespace App.ViewModels;

public sealed class ShowDescriptionMessage : ValueChangedMessage<Models.Task>
{
    public ShowDescriptionMessage(Models.Task task) : base(task) { }
}

public sealed class HideDescriptionMessage { }
public sealed class CreateTaskMessage
{
    public CreateTaskMessage(TaskListViewModel vm)
    {
        _vm = vm;
    }
    readonly TaskListViewModel _vm;
    public long AddTask(Task task)
    {
        return _vm.AddTask(task);
    }

}

public partial class TaskDescriprionViewModel : ObservableObject
{
    [ObservableProperty]
    public Models.Task task;

    public TaskDescriprionViewModel(Models.Task task)
    {
        WeakReferenceMessenger.Default.Register<ShowDescriptionMessage>(this, (r, m) => { Task = m.Value; });
        Task = task;
    }
    [RelayCommand]
    private void HideDescription()
    {
        WeakReferenceMessenger.Default.Send(new HideDescriptionMessage());
    }

    [RelayCommand]
    private void CreateTask()
    {
        var vm = App.Current.Handler.MauiContext.Services.GetService<TaskListViewModel>();
        WeakReferenceMessenger.Default.Send(new CreateTaskMessage(vm!));
    }
}
