using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.ViewModels;

namespace App.Views;

public partial class Menu : ContentView
{
    public Menu()
    {
        InitializeComponent();
        BindingContext = new MenuViewModel();
    }
}
