using App.Core.Services;
using App.Core.Services.Interfaces;
using App.Services.PrintService;
using App.Services.PrintService.impl;
using App.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace App.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }

    // Laufzettel Drucken
    private async void OnPrintButtonClicked(object sender, RoutedEventArgs e)
    {
        IPrintService printSerivce = new PrintService();
        await printSerivce.Print(ContentArea);
    }

    // Matrix Code generieren
    private void OnDataMatrixButtonClicked(object sender, RoutedEventArgs e)
    {
        IDataMatrixService dataMatrixService = new DataMatrixService();
        var data = text_test.Text;
        var path = @"C:\Logs";
        dataMatrixService.SaveDataMatrix(data, path);
    }
}