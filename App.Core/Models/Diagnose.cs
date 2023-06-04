using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Models
{
    public class Diagnose : BaseEntity
    {

        [Column(TypeName = "nvarchar(650)")]
        public string Name
        {
            get; set;
        }
        public List<Pcb> Pcbs
        {
            get; set;
        }

    }
}
