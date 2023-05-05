using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Models
{
    public class Comment : BaseEntity
    {

        [Required]
        [Column(TypeName = "nvarchar(650)")]
        public string Name
        {
            get; set;
        }
        [Required]
        public User NotedBy
        {
            get; set;
        }
        // TODO Foreign Key?
        public Pcb Pcb
        {
            get; set;
        }
    }
}
