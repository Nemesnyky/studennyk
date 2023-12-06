using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Repositories;
using App.Services;
using App.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace App.Views;
public sealed class HideNewTaskMessage { }

public partial class NewTask : ContentView
{
    private readonly IRepository repository;
    public NewTask()
    {
        InitializeComponent();
        BindingContext = new NewTaskViewModel();
        repository = AppServiceProvider.GetService<IRepository>();

    }



    private void CreateNewTask(object sender, TappedEventArgs e)
    {
        var offset = DateTimeOffset.Now.Offset;
        var date = new DateTimeOffset(Date.Date + Time.Time, offset);
        var task = new Models.Task(Models.Task.DEFAULT_ID, Title.Text, Description.Text, DateTimeOffset.Now, date, Models.Task.NOT_DONE);
        
        AppServiceProvider.GetService<AgendaViewModel>().AddTask(task);

        WeakReferenceMessenger.Default.Send(new HideSideBarMessage());


    }
}
