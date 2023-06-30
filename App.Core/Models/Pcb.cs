using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Models
{
    // Die Klasse "Pcb" erbt von der Basisklasse "BaseEntity" (Leiterplatte)
    public class Pcb : BaseEntity
    {
        // Die 10-Stellige Seriennummer des PCBs
        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        public string SerialNumber
        {
            get; set;
        }
        // Die Einschränkung des PCBs auf ein Gerät
        public Device Restriction
        {
            get; set;
        }
        // Die Fehlerbeschreibung des PCBs
        [Column(TypeName = "nvarchar(650)")]
        public string ErrorDescription
        {
            get; set;
        }
        // Eine Liste von Fehlerarten, die dem PCB zugeordnet sind
        public List<ErrorType> ErrorTypes
        {
            get; set;
        }
        // Gibt an, ob das PCB abgeschlossen ist oder nicht
        public bool Finalized
        {
            get; set;
        }
        // Die ID des PCB-Typs, dem das PCB zugeordnet ist (erforderlich)
        [Required]
        public int PcbTypeId
        {
            get; set;
        }
        // Der PCB-Typ, dem das PCB zugeordnet ist (Sachnummer)
        public PcbType PcbType
        {
            get; set;
        }
        // Die Anmerkung zum PCB
        [ForeignKey("CommentId")]
        public Comment Comment
        {
            get; set;
        }

        /// <summary>
        /// List of Transfers a pcb went through
        /// The List should always be orderd by ID or the field Created_At.
        /// The first Trasnfer is always the starting destination of the pcb
        /// The last Transfer is the End destination of the pcb but only if the pcb is finalized.
        /// /// </summary>
        public List<Transfer> Transfers
        {
            get; set;
        }
        // Die Diagnose des PCB
        [ForeignKey("DiagnoseId")]
        public Diagnose? Diagnose
        {
            get; set;
        }
        // Die ID der Diagnose, die dem PCB zugeordnet ist (nullable)
        public int? DiagnoseId
        {
            get; set;
        }


    }
}
