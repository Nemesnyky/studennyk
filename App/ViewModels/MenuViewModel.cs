using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace App.ViewModels;

public sealed class HideMenuMessage { }

public partial class MenuViewModel
{
    [RelayCommand]
    private void HideMenu()
    {
        WeakReferenceMessenger.Default.Send(new HideMenuMessage());
    }


}
