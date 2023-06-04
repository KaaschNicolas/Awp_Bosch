using App.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace App.Models;

public partial class TransferDTO : ObservableObject
{

    Transfer transfer;

    private DateTime _createdAt;
    public DateTime CreatedAt

    {
        get => _createdAt;
        set
        {
            // Bug Fix: when clicking on date of CalendarDatePicker twice
            // returns MinValue instead of null and set current value
            if (value == DateTime.MinValue)
            {
                OnPropertyChanged(nameof(CreatedAt));
                return;
            }
            else
            {
                SetProperty(ref _createdAt, value);
            }
        }
    }


    [ObservableProperty]
    private StorageLocation _storageLocationTransfer;

    [ObservableProperty]
    private User _notedBy;

    [ObservableProperty]
    private string _commentTransfer;

    public TransferDTO(Transfer transferToConvert)
    {

        transfer = transferToConvert;
        CreatedAt = transfer.CreatedDate;
        StorageLocationTransfer = transfer.StorageLocation;
        NotedBy = transfer.NotedBy;
        CommentTransfer = transfer.Comment;
    }

    public Transfer GetTransfer()
    {
        transfer.CreatedDate = CreatedAt;
        transfer.StorageLocation = StorageLocationTransfer;
        transfer.NotedBy = NotedBy;
        transfer.Comment = CommentTransfer;

        return transfer;
    }

}
