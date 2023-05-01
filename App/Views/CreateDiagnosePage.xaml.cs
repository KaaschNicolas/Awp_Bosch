using System.Xml.Linq;
using App.Behaviors;
using App.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Windows.Devices.Enumeration;

namespace App.Views;

public sealed partial class CreateDiagnosePage : Page
{
    public CreateDiagnoseViewModel ViewModel
    {
        get;
    }
    public CreateDiagnosePage()
    {
        ViewModel = App.GetService<CreateDiagnoseViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });

        DataContext = ViewModel;
    }

    private void TextBox_ValidationError(IInputValidationControl sender, InputValidationErrorEventArgs args)
    {

    }
}
