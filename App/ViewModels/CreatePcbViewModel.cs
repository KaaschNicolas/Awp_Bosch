using CommunityToolkit.Mvvm.ComponentModel;

namespace App.ViewModels;

public partial class CreatePcbViewModel : ObservableValidator
{
    [ObservableProperty]
    private string errorDate;

    public CreatePcbViewModel()
    {
    }
}
