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
    public class PcbType : BaseEntity
    {
        [Key]
        public int Id
        {
            get; set;
        }
        [Required]
        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        public string PcbPartNumber
        {
            get; set;
        }
        public int MaxTransfer
        {
            get; set;
        }

        public List<Pcb> Pcbs
        {
            get; set;
        }

    }
}
