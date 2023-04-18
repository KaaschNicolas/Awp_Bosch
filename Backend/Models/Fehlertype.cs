using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Fehlertype:BasisEntitaet
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }
        [Required]
        public string Fehlerbeschreibung { get; set; }

    }
}
