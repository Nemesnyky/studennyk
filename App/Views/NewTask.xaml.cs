using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.ViewModels;

namespace App.Views;

public partial class NewTask : ContentView
{
    public NewTask()
    {
        InitializeComponent();
        BindingContext = new NewTaskViewModel();

    }
}