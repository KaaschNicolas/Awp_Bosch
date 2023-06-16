using App.Core.Models;
using Microsoft.UI.Xaml;

namespace App.Contracts.Services;

public interface IDialogService
{
    Task<bool?> ConfirmDeleteDialogAsync(string title, string content, string confirmButtonText = "Löschen", string cancelButtonText = "Abbrechen");
    void UnAuthorizedDialogAsync(XamlRoot xamlRoot, string title, string content);

    Task<Response<Transfer>?> ShowCreateTransferDialog();

    Task<Comment> AddCommentDialog(string title, string confirmButtonText = "Speichern", string cancelButtonText = "Abbrechen");

    Task<Device> AddRestrictionDialog(string title, string confirmButtonText = "Speichern", string cancelButtonText = "Abbrechen");

    public Task<bool> RetryConnectionDialog(XamlRoot xamlRoot, string title, string confirmButtonText = "Neu Verbinden");
}
