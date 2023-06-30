using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Models
{
    // Die Klasse "ErrorType" erbt von der Basisklasse "BaseEntity" (Ober-& Unterfehler)
    public class ErrorType : BaseEntity
    {

        // Der Code des Fehlertyps (maximale Länge von 5 Zeichen)
        [MaxLength(5)]
        public string Code
        {
            get; set;
        }

        // Der Code des Fehlertyps (maximale Länge von 5 Zeichen)
        [Column(TypeName = "nvarchar(650)")]
        public string ErrorDescription
        {
            get; set;
        }
        // Das PCB, dem dieser Fehlertyp zugeordnet ist
        public Pcb Pcb
        {
            get; set;
        }

    }
}
