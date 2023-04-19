using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Nutzer : BasisEntitaet
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string AdUsername { get; set; }
    }
}
