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
    public class Leiterplatte : BasisEntitaet
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        public string SerienNummer { get; set; }
        public Geraet Einschraenkung { get; set; }
        [Column(TypeName = "nvarchar(650)")]
        public string Fehlerbeschreibung { get; set; }
        public List<Fehlertyp> Fehlertypen { get; set; }
        public bool Abgeschlossen { get; set; }
        [Required]
        public Leiterplattentyp Leiterplattentyp { get; set; }
        public Anmerkung Anmerkung { get; set; }
        [Required]
        public Umbuchung Einbuchung { get; set; }
        public List<Umbuchung> Weitergaben { get; set; }
        public Umbuchung EndgueltigerVerbleibOrt { get; set; }
        public Diagnose Enddiagnose { get; set; }

    }
}
