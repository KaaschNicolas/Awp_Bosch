using App.Core.Models;
using App.Core.Models.Enums;
using App.ViewModels;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System.Data;


namespace App.Views;

public sealed partial class PcbTypeI_OEvaluationPage : Page
{
    private List<CheckBox> _listCheckBox = new();

    private bool _wasChecked = true;
    private bool _canExecute = true;
    private enum DataGridDisplayMode
    {
        Default,
        Filtered
    }

    public PcbTypeI_OEvaluationViewModel ViewModel
    {
        get;
    }

    public PcbTypeI_OEvaluationPage()
    {
        ViewModel = App.GetService<PcbTypeI_OEvaluationViewModel>();
        InitializeComponent();
        DataContext = ViewModel;

        //var countRows = ViewModel.Table.Count;
        List<DataGridTextColumn> columns = new List<DataGridTextColumn>();
        //columns.Add()


        /*ListArea.Columns.Add(new Microsoft.UI.Xaml.Controls.DataGridTextColumn
        {
            Header = columnName,
            Binding = new Microsoft.UI.Xaml.Data.Binding(columName)
        });*/


        // Festlegen der Zeilen des DataGrids
        /*foreach (Dictionary<string, object> dict in ViewModel.Table)
        {
            ListArea.Items.Add(dict);
        }*/
    }

    private DataGridDisplayMode _displayMode = DataGridDisplayMode.Default;




    private void CheckBox_Loaded(object sender, RoutedEventArgs e)
    {

        CheckBox cb = sender as CheckBox;
        if (!_listCheckBox.Contains(cb))
        {
            _listCheckBox.Add(cb);
        }

        if ((_listCheckBox.Count - 1) == ViewModel.PtList.Count)
        {
            SelectAll_Checked(sender, e);
        }

    }

    private void Option_Checked(object sender, RoutedEventArgs e)
    {
        CheckBox cb = sender as CheckBox;
        if (cb.Content is not null)
        {
            PcbType checkedPcbType = ViewModel.PtList.Where(i => i.PcbPartNumber == (string)cb.Content).Single();
            ViewModel.SelectedPcbTypes.Add(checkedPcbType);

        }
        if (cb != _listCheckBox[0] && (ViewModel.SelectedPcbTypes.Count == (_listCheckBox.Count - 1)))
        {

            _listCheckBox[0].Checked -= SelectAll_Checked;
            _listCheckBox[0].IsChecked = true;
            _listCheckBox[0].Checked += SelectAll_Checked;
            _canExecute = false;
        }

    }

    private void Option_Unchecked(object sender, RoutedEventArgs e)
    {
        CheckBox cb = sender as CheckBox;
        ViewModel.SelectedPcbTypes.Remove(ViewModel.SelectedPcbTypes.Where(i => i.PcbPartNumber == (string)cb.Content).Single());
        if (cb != _listCheckBox[0])
        {
            _listCheckBox[0].Indeterminate -= SelectAll_Indeterminate;
            _listCheckBox[0].IsChecked = null;
            _listCheckBox[0].Indeterminate += SelectAll_Indeterminate;
        }

        if (ViewModel.SelectedPcbTypes.Count == 0)
        {
            _listCheckBox[0].IsChecked = false;

            _canExecute = true;
        }
    }

    private void SelectAll_Checked(object sender, RoutedEventArgs e)
    {
        _wasChecked = true;

        foreach (CheckBox cb in _listCheckBox)
        {
            cb.IsChecked = true;
        }



    }

    private void SelectAll_Unchecked(object sender, RoutedEventArgs e)
    {
        
        foreach (CheckBox cb in _listCheckBox)
        {
            cb.IsChecked = false;
        }
        ViewModel.SelectedPcbTypes.Clear();
        _wasChecked = false;
    }


    private void SelectAll_Indeterminate(object sender, RoutedEventArgs e)
    {
        if (_wasChecked)
        {
            SelectAll_Unchecked(sender, e);
            _listCheckBox[0].Unchecked -= SelectAll_Unchecked;
            _listCheckBox[0].IsChecked = false;
            _listCheckBox[0].Unchecked += SelectAll_Unchecked;
        }
        else
        {
            SelectAll_Checked(sender, e);
            _listCheckBox[0].Checked -= SelectAll_Checked;
            _listCheckBox[0].IsChecked = true;
            _listCheckBox[0].Checked += SelectAll_Checked;
        }

    }

    private async void FilterPcbTypes_Click(object sender, RoutedEventArgs e)
    {
        _displayMode = DataGridDisplayMode.Filtered;
        ViewModel.FilterOptions = PcbFilterOptions.FilterPcbTypes;
        await ViewModel.FilterItems.ExecuteAsync(null);
    }

    private async void FilterClear_Click(object sender, RoutedEventArgs e)
    {
        _displayMode = DataGridDisplayMode.Default;
        ViewModel.FilterOptions = PcbFilterOptions.None;
        SelectAll_Checked(sender, e);
        await ViewModel.FilterItems.ExecuteAsync(null);
    }


    private void StartDateChanged(object sender, CalendarDatePickerDateChangedEventArgs e)
    {
        ViewModel.StartDate = ((DateTimeOffset)Start.Date).DateTime;
    }

    private void EndDateChanged(object sender, CalendarDatePickerDateChangedEventArgs e)
    {
        ViewModel.EndDate = ((DateTimeOffset)End.Date).DateTime;
    }


    private void Click_EvaluationButton(object sender, RoutedEventArgs e)
    {

        var dt = new DataTable();
        int columnCount = ViewModel.Header.Count;

        for (int i = 0; i < columnCount; i++)
        {
            dt.Columns.Add(ViewModel.Header[i]);
            ListArea.Columns.Add(new DataGridTextColumn()
            {
                Header = dt.Columns[i].ColumnName,
                Binding = new Binding { Path = new PropertyPath("[" + i.ToString() + "]") }
            });
        }


        for (int i = 0; i < ViewModel.Rows.Count; i++)
            dt.Rows.Add(ViewModel.Rows[i].Take(columnCount).ToArray());

        var collectionObjects = new System.Collections.ObjectModel.ObservableCollection<object>();
        foreach (DataRow row in dt.Rows)
        {
            collectionObjects.Add(row.ItemArray);
        }
        ListArea.ItemsSource = collectionObjects;
    }
}
