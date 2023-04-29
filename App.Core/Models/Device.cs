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
    public class Device : BaseEntity
    {
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Name
        {
            get; set;
        }
        public List<Pcb> Pcbs
        {
            get; set;
        }
    }
}
