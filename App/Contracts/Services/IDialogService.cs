namespace App.Contracts.Services;

public interface IDialogService
{
    Task<bool?> ConfirmDeleteDialogAsync(string title, string content, string confirmButtonText = "Löschen", string cancelButtonText = "Abbrechen");
    void UnAuthorizedDialogAsync(string title, string content, XamlRoot xamlRoot);

    Task<Tuple<Transfer, int?>?> ShowCreateTransferDialog(string title, string confirmButtonText = "Weitergeben", string cancelButtonText = "Abbrechen");

    Task<Comment> AddCommentDialog(string title, string confirmButtonText = "Speichern", string cancelButtonText = "Abbrechen");

    Task<Device> AddRestrictionDialog(string title, string confirmButtonText = "Speichern", string cancelButtonText = "Abbrechen");
}
