using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.ViewModels;

namespace App.Views;

public partial class TaskDescription : ContentView
{
    public TaskDescription(Models.Task task)
    {
        InitializeComponent();
        BindingContext = new TaskDescriprionViewModel(task);
    }
}