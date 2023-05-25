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

namespace App.Views;

public sealed partial class PcbSinglePage : Page
{
    public PcbSingleViewModel ViewModel
    {
        get;
    }

    //private static BitmapImage dataMatrixImageSource = new BitmapImage();
    //private static ImageBrush dataMatrixImageBrush = new ImageBrush();

    public PcbSinglePage()
    {
        ViewModel = App.GetService<PcbSingleViewModel>();
        InitializeComponent();
        DataContext = ViewModel;
        //dateOfFailure.Date = new DateTime(2023, 04, 23);
        // GenerateBarcode();
        showRestrictionButton();
        //colorDaysInCirculation();
    }

    public void showRestrictionButton()
    {
        if(ViewModel.Restriction == null)
        {
            RestrictionInfoBar.Visibility = Visibility.Collapsed;
            RestrictionButton.Visibility = Visibility.Visible;    
        }
        else
        {
            RestrictionInfoBar.Visibility = Visibility.Visible;
            RestrictionButton.Visibility = Visibility.Collapsed;
        }
    }

    public void colorDaysInCirculation()
    {
        if(ViewModel.ColorDays == "green") 
        {
            inCirculationText.Background = new SolidColorBrush(Colors.Green);
            Days.Background = new SolidColorBrush(Colors.Green);
        }
        else if(ViewModel.ColorDays == "yellow")
        {
            inCirculationText.Background = new SolidColorBrush(Colors.Yellow);
            Days.Background = new SolidColorBrush(Colors.Yellow);
        }
        else if(ViewModel.ColorDays == "red")
        {
            inCirculationText.Background = new SolidColorBrush(Colors.Red);
            Days.Background = new SolidColorBrush(Colors.Red);
        }
    }

    void TransferClick(object sender, RoutedEventArgs e)
    {
        ViewModel.ShowTransferCommand.Execute(null);
    }

    //    private async void OnPrintButtonClicked(object sender, RoutedEventArgs e)
    //    {
    //        /*var data = new PageData(this, dataMatrixRectangle);
    //        var rect = (Rectangle)data.Rectangle.FindName("dataMatrixRectangle");
    //        rect.Fill = dataMatrixImageBrush;
    //        var page = data.Page;
    //        var pageTest = new Page();
    //        var layoutControl = new Grid();
    //        //layoutControl.Children.Add(uiElementGrid1);
    //        layoutControl.Children.Add(uiElementGrid2);
    //        pageTest.Content = layoutControl;*/

    //        IPrintService printSerivce = new PrintService();
    //        await printSerivce.Print(PcbSinglePageContent); //Test
    //    }

    //    private void GenerateBarcode()
    //    {
    //        IDataMatrixService dms = new DataMatrixService();
    //        var dataMatrixImageSource = BitmapToBitmapImageConverter.GetBitmapImage(dms.GetDataMatrix(ViewModel.SerialNumber));
    //        ImageBrush dataMatrixImageBrush = new ImageBrush();
    //        dataMatrixImageBrush.ImageSource = dataMatrixImageSource;
    //        dataMatrixRectangle.Fill = dataMatrixImageBrush;
    //    }
}
