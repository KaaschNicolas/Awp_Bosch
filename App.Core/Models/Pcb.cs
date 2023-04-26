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
    public class Pcb : BaseEntity
    {
        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        public string SerialNumber
        {
            get; set;
        }
        public Device Restriction
        {
            get; set;
        }
        [Column(TypeName = "nvarchar(650)")]
        public string ErrorDescription
        {
            get; set;
        }
        public List<ErrorType> ErrorTypes
        {
            get; set;
        }
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
        /// Liste der Weitergaben einer Laeterplatte
        /// Die Reihenfolge gilt nach ID bzw. Created_At
        /// Die erste Weitergabe ist die Einbuchung
        /// Ist die Leiterplatte abgeschlossen,
        /// so ist die letzte Umbunchung der EndgültigeVerbleibOrt
        /// </summary>
        public List<Transfer> Transfers
        {
            get; set;
        }
        public Diagnose Enddiagnose
        {
            get; set;
        }

    }
}
