using App.Repositories;
using App.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

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
        }
    }
}
