﻿using App.Contracts.Services;
using App.Controls;
using App.Core.Models;
using App.Core.Models.Enums;
using App.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace App.Services;

/// <summary>
/// A <see langword="class"/> that implements the <see cref="IDialogService"/> <see langword="interface"/> using WinUI APIs.
/// </summary>
public sealed class DialogService : IDialogService
{
    /// <inheritdoc/>
    private readonly FrameworkElement? rootElement = App.MainWindow.Content as FrameworkElement;

    // Methode zum Anzeigen eines Bestätigungsdialogs für das Löschen eines Objekts.
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
    // Methode zum Anzeigen eines Dialogs zur Erstellung einer Weitergabe.
    public async Task<Response<Transfer>?> ShowCreateTransferDialog()
    {
        if (rootElement != null)
        {
            /*  WindowEx window = new TransferWindow();
              window.Activate();*/

            var dialog = new TransferDialog
            {
                XamlRoot = rootElement.XamlRoot,
                RequestedTheme = rootElement.RequestedTheme
            };

            var result = await dialog.ShowAsync();

            TransferDialogViewModel vm = dialog.ViewModel;

            if (result == ContentDialogResult.None)
            {
                return new Response<Transfer>(ResponseCode.None, "None");
            }
            if (result == ContentDialogResult.Primary)
            {
                return await vm.Save();
            }
        }
        return null;
    }

    // Methode zum Hinzufügen einer Anmerkung durch einen Dialog.
    public async Task<Comment> AddCommentDialog(string title, string confirmButtonText, string cancelButtonText)
    {
        if (rootElement != null)
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



    // Methode zum Hinzufügen einer Einschränkung durch einen Dialog.
    public async Task<Device> AddRestrictionDialog(string title, string confirmButtonText, string cancelButtonText)
    {
        if (rootElement != null)
        {
            TextBox restriction = new TextBox
            {
                Header = "Einschränkung",
                PlaceholderText = "Text eintragen"
            };

            var dialog = new ContentDialog
            {
                Title = title,
                Content = restriction,
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

                return new Device
                {
                    Name = restriction.Text
                };
            }

        }
        return null;
    }



    // Methode zum Anzeigen eines Dialogs, wenn ein user nicht autorisiert ist.
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