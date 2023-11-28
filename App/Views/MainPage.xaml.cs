using App.Repositories;
using App.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using System;

namespace App.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<HideMenuMessage>(this, (r, m) =>
            {
                layout.RemoveAt(0);
            });
            WeakReferenceMessenger.Default.Register<ShowDescriptionMessage>(this, (r, m) =>
            {
                layout.Insert(2, description);
            });
        }
    }
}
