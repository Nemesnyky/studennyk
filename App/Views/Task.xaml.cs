using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Views;

public partial class Task : ContentView
{
    public Task()
    {
        InitializeComponent();
    }

    public string Text
    {
        get => TaskButton.Text;
        set => TaskButton.Text = value;
    }
}