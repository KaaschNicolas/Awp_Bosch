using System.Diagnostics;
using App.Behaviors;
using App.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace App.Views;
public sealed partial class UpdateDiagnosePage : Page
{
    public UpdateDiagnoseViewModel ViewModel
    {
        get;
    }

    public UpdateDiagnosePage()
    {
        ViewModel = App.GetService<UpdateDiagnoseViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }
    
}

