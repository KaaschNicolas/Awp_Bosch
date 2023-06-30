using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Models
{
    // Die Klasse "Transfer" erbt von der Basisklasse "BaseEntity" (Weitergabe/Umbuchungen)
    public class Transfer : BaseEntity
    {

        // Die Anmerkung der Umbuchung
        public string Comment

        {
            get; set;
        }

        // Die ID der Lagerort-Eigenschaft, des Lagerorts an den das PCB umbegbucht wurde
        [Required]
        public int StorageLocationId { get; set; }

        // Der Lagerort, an den das PCB verbucht wurde
        [Required]
        public StorageLocation StorageLocation
        {
            get; set;
        }


        // Die ID der "NotedBy"-Eigenschaft, die den Benutzer repräsentiert, der die Umbuchug vermerkt hat
        [Required]
        public int NotedById { get; set; }


        // Die "NotedBy"-Eigenschaft, die den Benutzer repräsentiert, der die Umbuchung vermerkt hat
        public User NotedBy
        {
            get; set;
        }


        // Die PCB-ID-Eigenschaft zum PCB das umgebucht wurde
        public int PcbId
        {
            get; set;
        }
        
        // Das PCB das umgebucht wurde
        [ForeignKey(nameof(PcbId))]
        public Pcb Pcb
        {
            get; set;
        }
    }
}
