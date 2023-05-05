using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Models
{
    public class Pcb : BaseEntity
    {
        // TODO: Ausgefallen am fehlt als Feld, da Unterschiedlich zu Weitergabe Datum, wenn ich mich nicht irre

        // [Required]
        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        public string SerialNumber
        {
            get; set;
        }

        //TODO: Sinnvoll nicht einfach als String hier zu speichern anstatt in anderer Tabelle?
        // Wenn ja dann fehlt FK
        public Device Restriction
        {
            get; set;
        }

        // TODO: Was macht das Feld, Fehlerbeschreibung müsste in ErrorTypes drin sein.
        [Column(TypeName = "nvarchar(650)")]
        public string ErrorDescription
        {
            get; set;
        }

        // TODO: ErrorTypes vielleicht Entity umbenennen in Error? da es ja nicht nur der Error-Typ ist.
        public List<ErrorType> ErrorTypes
        {
            get; set;
        }

        // TODO: Standardwert = False
        // Required
        public bool Finalized
        {
            get; set;
        }
        [Required]
        public PcbType PcbType
        {
            get; set;
        }
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
        public Diagnose Diagnose
        {
            get; set;
        }

    }
}
