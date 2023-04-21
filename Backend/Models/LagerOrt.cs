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
    public class LagerOrt : BasisEntitaet
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string LagerName { get; set; }
        public int VerweildauerGelb { get; set; }
        public int VerweildauerRot { get; set; }
        public List<Umbuchung> Umbuchungen { get; set; }

    }
}
