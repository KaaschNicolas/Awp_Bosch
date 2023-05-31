using App.Core.Models;
using Microsoft.UI.Xaml;
namespace App.Contracts.Services;
public interface IDialogService
{
    //TODO Extend the Interface -> InputDialog 
    Task<bool?> ConfirmDeleteDialogAsync(string title, string content, string confirmButtonText = "Löschen", string cancelButtonText = "Abbrechen");
    void UnAuthorizedDialogAsync(string title, string content, XamlRoot xamlRoot);

    Task<Tuple<Transfer, int>?> ShowCreateTransferDialog(string title, User user, List<StorageLocation> storageLocations, List<Diagnose> diagnoses, string confirmButtonText = "Weitergeben", string cancelButtonText = "Abbrechen");

    Task<Comment> AddCommentDialog(string title, string confirmButtonText = "Speichern", string cancelButtonText = "Abbrechen");
}
