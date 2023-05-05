using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //TODO: Required
        public int DwellTimeYellow
        {
            get; set;
        }
        //TODO: Required
        public int DwellTimeRed
        {
            get; set;
        }
        public List<Transfer> Transfers
        {
            get; set;
        }

    }
}
