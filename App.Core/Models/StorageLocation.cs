using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Models
{
    public class StorageLocation : BaseEntity
    {
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string StorageName
        {
            get; set;
        }
        public int DwellTimeYellow
        {
            get; set;
        }
        public int DwellTimeRed
        {
            get; set;
        }
        public bool IsFinalDestination
        {
            get; set;
        }
        public List<Transfer> Transfers
        {
            get; set;
        }

    }
}
