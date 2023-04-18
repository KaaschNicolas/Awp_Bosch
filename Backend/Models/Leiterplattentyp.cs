using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Leiterplattentyp : BasisEntitaet
    {
        public string Id { get; set; }                
        [Required]
        public string LpSachnummer { get; set; }
        public int MaxWeitergaben { get; set; }           
    }
}
