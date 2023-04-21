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
    public class Leiterplattentyp : BasisEntitaet
    {
        [Key]
        public string Id { get; set; }                
        [Required]
        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        public string LpSachnummer { get; set; }
        public int MaxWeitergaben { get; set; }           
        public List<Leiterplatte> Leiterplatten { get; set; }

    }
}
