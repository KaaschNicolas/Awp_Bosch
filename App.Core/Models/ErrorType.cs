using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Models
{
    public class ErrorType : BaseEntity
    {
        [Required]
        [MaxLength(5)]
        // TODO: Code immer nur max. 5?
        public string Code
        {
            get; set;
        }

        // TODO: Rechtschreibfehler ändern
        [Required]
        [Column(TypeName = "nvarchar(650)")]
        public string ErrorDescribtion
        {
            get; set;
        }
        public List<Pcb> Pcbs
        {
            get; set;
        }

    }
}
