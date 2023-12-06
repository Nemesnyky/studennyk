using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Services;
using App.ViewModels;
using CommunityToolkit.Mvvm.Messaging;


namespace App.Views
{
    public partial class Agenda : ContentView
    {
        public Agenda()
        {
            InitializeComponent();
            BindingContext =  AppServiceProvider.GetService<AgendaViewModel>();
        }

    }

}
