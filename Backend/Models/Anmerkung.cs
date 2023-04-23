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
    public class Anmerkung : BasisEntitaet
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(650)")]
        public string Name { get; set; }
        [Required]
        public Nutzer VermerktVon {get; set;}
        
        public Leiterplatte Leiterplatte { get; set; }
    }
}
