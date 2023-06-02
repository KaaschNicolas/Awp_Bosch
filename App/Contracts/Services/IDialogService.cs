using App.Core.Models;
using Microsoft.UI.Xaml;
namespace App.Contracts.Services;
public interface IDialogService
{
    Task<bool?> ConfirmDeleteDialogAsync(string title, string content, string confirmButtonText = "Löschen", string cancelButtonText = "Abbrechen");
    void UnAuthorizedDialogAsync(string title, string content, XamlRoot xamlRoot);
    Task<Comment> AddCommentDialog(string title, string confirmButtonText = "Speichern", string cancelButtonText = "Abbrechen");
    Task<Device> AddRestrictionDialog(string title, string confirmButtonText = "Speichern", string cancelButtonText = "Abbrechen");
    Task<Tuple<Transfer, int?>?> ShowCreateTransferDialog(string title, string confirmButtonText = "Weitergeben", string cancelButtonText = "Abbrechen");
}
