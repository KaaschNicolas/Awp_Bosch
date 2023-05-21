using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Models
{
    public class ErrorType : BaseEntity
    {
        [Required]
        [MaxLength(5)]
        public string Code
        {
            get; set;
        }
        [Required]
        [Column(TypeName = "nvarchar(650)")]
        public string ErrorDescription
        {
            get; set;
        }
        public Pcb Pcb
        {
            get; set;
        }

    }
}
