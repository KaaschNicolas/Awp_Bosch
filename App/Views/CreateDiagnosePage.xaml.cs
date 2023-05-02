using System.Xml.Linq;
using App.Behaviors;
using App.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Windows.Devices.Enumeration;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class CreateDiagnosePage : Page
{
    public DiagnoseViewModel ViewModel
    {
        get;
    }
    public CreateDiagnosePage()
    {
        DataContext = App.GetService<DiagnoseViewModel>();
        InitializeComponent();
        SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, new Binding
        {
            Source = ViewModel,
            Mode = BindingMode.OneWay
        });
    }


}
