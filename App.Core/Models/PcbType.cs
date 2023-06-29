using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Models
{
    // Die Klasse "PcbType" erbt von der Basisklasse "BaseEntity" (Sachnummer)
    public class PcbType : BaseEntity
    {

        // Die PCB-Sachnummer-Eigenschaft 
        [Required]
        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        public string PcbPartNumber
        {
            get; set;
        }

        // Die Beschreibung des PCB-Typs 
        [Required]
        public string Description
        {
            get; set;
        }

        // Die maximale Anzahl von Umbuchungen an für diesen PCB-Typ
        [Required]
        public int MaxTransfer
        {
            get; set;
        }
        // Eine Liste von PCBs, die diese PCB-Typ haben.
        public List<Pcb> Pcbs
        {
            get; set;
        }

    }
}
