using App.Contracts.Services;
using App.Core.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Security.Cryptography.X509Certificates;


namespace App.Services;

/// <summary>
/// A <see langword="class"/> that implements the <see cref="IDialogService"/> <see langword="interface"/> using WinUI APIs.
/// </summary>
public sealed class DialogService : IDialogService
{
    /// <inheritdoc/>
    private readonly FrameworkElement? rootElement = App.MainWindow.Content as FrameworkElement;

    public async Task<bool?> ConfirmDeleteDialogAsync(string title, string content, string confirmButtonText, string cancelButtonText)
    {
        if (rootElement != null)

        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                PrimaryButtonText = confirmButtonText,
                DefaultButton = ContentDialogButton.Close,
                RequestedTheme = rootElement.RequestedTheme,
                CloseButtonText = cancelButtonText,
                XamlRoot = rootElement.XamlRoot

            };
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.None)
            {
                return null;
            }

            return (result == ContentDialogResult.Primary);
        }
        return null;
    }
    public async Task<Tuple<Transfer, int>?> ShowCreateTransferDialog(string title, User user, List<StorageLocation> storageLocations, List<Diagnose> diagnoses, string confirmButtonText, string cancelButtonText)
    {
        if (rootElement != null)

        {
            Grid DynamicGrid = new Grid { RowSpacing = 20 };
            ColumnDefinition gridCol1 = new ColumnDefinition();
            ColumnDefinition gridCol2 = new ColumnDefinition();

            DynamicGrid.ColumnDefinitions.Add(gridCol1);
            DynamicGrid.ColumnDefinitions.Add(gridCol2);

            RowDefinition gridRow1 = new RowDefinition();
            RowDefinition gridRow2 = new RowDefinition();
            RowDefinition gridRow3 = new RowDefinition();

            DynamicGrid.RowDefinitions.Add(gridRow1);
            DynamicGrid.RowDefinitions.Add(gridRow2);
            DynamicGrid.RowDefinitions.Add(gridRow3);

            CalendarDatePicker transferDate = new CalendarDatePicker
            {
                Header = "Eingang",
                Date = DateTime.Today
            };
            Grid.SetRow(transferDate, 0);
            Grid.SetColumn(transferDate, 0);
            DynamicGrid.Children.Add(transferDate);

            TextBox username = new TextBox
            {
                Header = "aufgenommen von:",
                Text = user.Name,
                IsReadOnly = true,
            };
            Grid.SetRow(username, 0);
            Grid.SetColumn(username, 1);
            DynamicGrid.Children.Add(username);

            ComboBox storageLocation = new ComboBox
            {
                Header = "Ort",
                PlaceholderText = "auswählen",
                ItemsSource = storageLocations,
                DisplayMemberPath = "StorageName"

            };
            Grid.SetRow(storageLocation, 1);
            Grid.SetColumn(storageLocation, 0);
            DynamicGrid.Children.Add(storageLocation);


            ComboBox diagnose = new ComboBox
            {
                Header = "Fehlerkategorie",
                PlaceholderText = "auswählen",
                ItemsSource = diagnoses,
                DisplayMemberPath = "Name"


            };
            Grid.SetRow(diagnose, 1);
            Grid.SetColumn(diagnose, 1);
            DynamicGrid.Children.Add(diagnose);

            TextBox comment = new TextBox
            {
                Header = "Beurteilung | Anmerkung",
                PlaceholderText = "Text eintragen"
            };

            Grid.SetRow(comment, 2);
            Grid.SetColumnSpan(comment, 2);
            DynamicGrid.Children.Add(comment);


            var dialog = new ContentDialog
            {
                Title = title,
                Content = DynamicGrid,
                PrimaryButtonText = confirmButtonText,
                DefaultButton = ContentDialogButton.Primary,
                RequestedTheme = rootElement.RequestedTheme,
                CloseButtonText = cancelButtonText,
                XamlRoot = rootElement.XamlRoot

            };
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.None)
            {
                return null;
            }
            if (result == ContentDialogResult.Primary)
            {

                return Tuple.Create(new Transfer
                {
                    NotedById = user.Id,
                    CreatedDate = transferDate.Date.Value.DateTime,
                    StorageLocationId = ((StorageLocation)storageLocation.SelectedItem).Id,
                    Comment = comment.Text
                }, ((Diagnose)diagnose.SelectedItem).Id);
            }

        }
        return null;
    }

    public async Task<Comment> AddCommentDialog(string title, string confirmButtonText, string cancelButtonText)
    {
        if(rootElement != null)
        {
            TextBox comment = new TextBox
            {
                Header = "Anmerkung",
                PlaceholderText = "Text eintragen"
            };

            var dialog = new ContentDialog
            {
                Title = title,
                Content = comment,
                PrimaryButtonText = confirmButtonText,
                DefaultButton = ContentDialogButton.Primary,
                RequestedTheme = rootElement.RequestedTheme,
                CloseButtonText = cancelButtonText,
                XamlRoot = rootElement.XamlRoot

            };
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.None)
            {
                return null;
            }
            if (result == ContentDialogResult.Primary)
            {

                return new Comment
                {
                    Content = comment.Text
                };
            }

        }
        return null;
    }

    public async void UnAuthorizedDialogAsync(string title, string content, XamlRoot xamlRoot)
    {

        if (xamlRoot != null)
        {
            ContentDialog unAuthorizedDialog = new ContentDialog()
            {
                Title = title,
                Content = content,
                XamlRoot = xamlRoot
            };

            await unAuthorizedDialog.ShowAsync();
        }

    }

}