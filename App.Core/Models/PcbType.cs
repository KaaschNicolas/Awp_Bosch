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
        new public string Id { get; set; }

        [Required]
        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        public string PcbPartNumber
        {
            get; set;
        }

        [Required]
        public string Description { get; set; }

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
