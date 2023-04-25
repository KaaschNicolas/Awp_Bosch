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
        [Key]
        public int Id
        {
            get; set;
        }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string LagerName
        {
            get; set;
        }
        public int VerweildauerGelb
        {
            get; set;
        }
        public int VerweildauerRot
        {
            get; set;
        }
        public List<Transfer> Umbuchungen
        {
            get; set;
        }

    }
}
