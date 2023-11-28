using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace App.ViewModels;

public sealed class ShowDescriptionMessage
{
    public ShowDescriptionMessage(int taskId)
    {
    }
}

public partial class TaskDescriprionViewModel
{
    [RelayCommand]
    public void ShowDescription(int taskId)
    {
        WeakReferenceMessenger.Default.Send(new ShowDescriptionMessage(taskId));
    }


}
