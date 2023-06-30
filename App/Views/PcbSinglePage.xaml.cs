using App.Services.PrintService.impl;
using App.Services.PrintService;
using App.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using App.Core.Services.Interfaces;
using App.Core.Services;
using App.Helpers;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Shapes;
using System.Drawing.Imaging;
using System.Net;
using Windows.Graphics.Printing;
using Windows.UI;
using Microsoft.UI;
using App.Core.Models.Enums;

namespace App.Views;

public sealed partial class PcbSinglePage : Page
{
    public PcbSingleViewModel ViewModel
    {
        get;
    }

    public PcbSinglePage()
    {
        ViewModel = App.GetService<PcbSingleViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
    }

    private void EditClick(object sender, RoutedEventArgs e)
    {
        if(AuthServiceHelper.hasRole(Role.Admin) || AuthServiceHelper.hasRole(Role.StandardUser))
        {
            ViewModel.EditCommand.Execute(null);
        }
        else { }
        
    }

    private void DeleteClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (AuthServiceHelper.hasRole(Role.Admin))
        {
            ViewModel.DeleteCommand.Execute(null);
        }
        else { }
    }

    private void PrintClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        if (AuthServiceHelper.hasRole(Role.Admin) || AuthServiceHelper.hasRole(Role.StandardUser))
        {
            ViewModel.PrintCommand.Execute(SinglePage);
        }
        else { }

    }

    void TransferClick(object sender, RoutedEventArgs e)
    {
        if (AuthServiceHelper.hasRole(Role.Admin) || AuthServiceHelper.hasRole(Role.StandardUser))
        {
            ViewModel.ShowTransferCommand.Execute(null);
        }
        else { }
    }

    void CommentClick(object sender, RoutedEventArgs e)
    {
        if (AuthServiceHelper.hasRole(Role.Admin) || AuthServiceHelper.hasRole(Role.StandardUser))
        {
            ViewModel.AddCommentCommand.Execute(null);
        }
        else { }
    }

    void RestrictionClick(object sender, RoutedEventArgs e)
    {
        if (AuthServiceHelper.hasRole(Role.Admin) || AuthServiceHelper.hasRole(Role.StandardUser))
        {
            ViewModel.AddRestrictionCommand.Execute(null);
        }
        else { }
    }
}
