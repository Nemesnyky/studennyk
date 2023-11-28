using App.Repositories;
using App.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using System;


namespace App.Views
{
    public partial class MainPage : ContentPage
    {
        private bool showDescription = false;
        public MainPage()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<HideMenuMessage>(this, (r, m) =>
            {
                grid.RemoveAt(0);
                grid.ColumnDefinitions.RemoveAt(0);
                grid.SetColumn(agenda, 0);
            });
            WeakReferenceMessenger.Default.Register<ShowDescriptionMessage>(this, (r, m) =>
            {
                if (showDescription) { return; }
                grid.AddColumnDefinition(new ColumnDefinition() { Width = new GridLength(0.25, GridUnitType.Star) });
                grid.Add(new TaskDescription(m.Value) , grid.ColumnDefinitions.Count - 1 , 0);
                showDescription = true; 
            });
        }
    }
}
