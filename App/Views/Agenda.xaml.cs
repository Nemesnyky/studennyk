using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Views;

public partial class Agenda : ContentView
{
    private AgendaViewModel _viewModel = new();
    
    public Agenda()
    {
        InitializeComponent();
        
        System.Threading.Tasks.Task.Run(async () =>
        {
            await _viewModel.LoadTasks(0);

            Console.WriteLine($"LOADED {_viewModel.Tasks.Count} TASKS");

            foreach (var task in _viewModel.Tasks)
            {
                Application.Current?.Dispatcher.Dispatch(() =>
                {
                    var taskView = new Task
                    {
                        Text = task.Title
                    };
                    TaskList.Add(taskView);
                });
            }
        });
    }
}