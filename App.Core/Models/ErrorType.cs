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
    public class ErrorType : BaseEntity
    {
        [Required]
        [MaxLength(5)]
        public string Code
        {
            get; set;
        }
        [Required]
        [Column(TypeName = "nvarchar(650)")]
        public string ErrorDescribtion
        {
            get; set;
        }
        public List<Pcb> Pcbs
        {
            get; set;
        }

    }
}
