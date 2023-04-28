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
