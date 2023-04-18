using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public  class Leiterplatte
    {
        public int Id { get; set; }
        public string SerienNummer { get; set; }
        public Geraet Einschraenkung { get; set; }
        public string Fehlerbeschreibung { get; set; }
        public Fehlertype[] Fehlertypen { get; set; }
        public bool Abgeschlossen { get; set; }
        [Required]
        public Leiterplattentyp Leiterplattentyp { get; set; }
        public Anmerkung Anmerkung { get; set; }
        [Required]
        public Umbuchung Einbuchung { get; set; }
        public List<Umbuchung> Weitergaben { get; set; }
        public Umbuchung EndgueltigerVerbleibOrt { get; set; }
    }
}
