using App.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace App.Views;

public sealed partial class TransfersPage : Page
{
    public TransfersViewModel ViewModel
    {
        get;
    }

    public TransfersPage()
    {
        ViewModel = App.GetService<TransfersViewModel>();
        InitializeComponent();
    }

    private void CalendarDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs e)
    {
        CalendarDatePicker calendarDatePicker = sender as CalendarDatePicker;   
    }
}
