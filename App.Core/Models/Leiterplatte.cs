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
    public class Leiterplatte : BasisEntitaet
    {
        [Key]
        public int Id
        {
            get; set;
        }
        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        public string SerienNummer
        {
            get; set;
        }
        public Geraet Einschraenkung
        {
            get; set;
        }
        [Column(TypeName = "nvarchar(650)")]
        public string Fehlerbeschreibung
        {
            get; set;
        }
        public List<Fehlertyp> Fehlertypen
        {
            get; set;
        }
        public bool Abgeschlossen
        {
            get; set;
        }
        [Required]
        public Leiterplattentyp Leiterplattentyp
        {
            get; set;
        }
        [ForeignKey("AnmerkungId")]
        public Anmerkung Anmerkung
        {
            get; set;
        }

        /// <summary>
        /// Liste der Weitergaben einer Laeterplatte
        /// Die Reihenfolge gilt nach ID bzw. Created_At
        /// Die erste Weitergabe ist die Einbuchung
        /// Ist die leiterplatte abgeschlossen,
        /// so ist die letzte Umbunchung der EndgültigeVerbleibOrt
        /// </summary>
        public List<Umbuchung> Weitergaben
        {
            get; set;
        }
        public Diagnose Enddiagnose
        {
            get; set;
        }

    }
}
