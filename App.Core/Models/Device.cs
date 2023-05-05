using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Models
{
    public class Device : BaseEntity
    {
        //TODO: Ist die Entität nötig? Haben wir irgendein Vorteil davon, wenn wir es nicht direkt in Pcb-Model speichern?
        // Eigentlich auch keine Stammdaten dafür und wenn 1:N dann stammdaten nötig oder funktion wenn der eintrag nicht existiert anzulegen,
        // wird aber schwierig mit autosuggestion control
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Name
        {
            get; set;
        }
        public List<Pcb> Pcbs
        {
            get; set;
        }
    }
}
