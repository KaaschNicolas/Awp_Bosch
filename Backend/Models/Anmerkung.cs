using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Anmerkung : BasisEntitaet
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Nutzer VermerktVon {get; set;} 
    }
}
