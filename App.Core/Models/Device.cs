using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Models
{
    // Die Klasse "Device" erbt von der Basisklasse "BaseEntity" (Einschränkung)
    public class Device : BaseEntity
    {
        // Der Name des Geräts
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Name
        {
            get; set;
        }
        // Eine Liste von PCBs, die diesem Gerät als Einschränkung besitzen
        public List<Pcb> Pcbs
        {
            get; set;
        }
    }
}
