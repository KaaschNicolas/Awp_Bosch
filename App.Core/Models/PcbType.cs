using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Models
{
    public class PcbType : BaseEntity
    {

        [Required]
        [MaxLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        public string PcbPartNumber
        {
            get; set;
        }

        [Required]
        public string Description
        {
            get; set;
        }
        [Required]

        public int MaxTransfer
        {
            get; set;
        }
        public List<Pcb> Pcbs
        {
            get; set;
        }

    }
}
