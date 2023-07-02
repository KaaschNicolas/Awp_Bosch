using App.Core.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Models
{
    // Die Klasse "User" erbt von der Basisklasse "BaseEntity" (Benutzer)
    public class User : BaseEntity
    {
        // Die Eigenschaft "Name" des Benutzers
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string Name
        {
            get; set;
        }
        // Die Eigenschaft "AdUsername" des Benutzers (NT-Username)
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string AdUsername
        {
            get; set;
        }
        // Die Eigenschaft "Role" des Benutzers (Berechtigung)
        [MaxLength(24)]
        [Column(TypeName = "nvarchar(24)")]
        public Role Role { get; set; }
       
        // Eine Liste von "Transfer"-Objekten, die dem Benutzer zugeordnet sind (Weitergaben/Umbuchungen)
        public List<Transfer> Transfers
        {
            get; set;
        }
        // Eine Liste von "Comment"-Objekten, die dem Benutzer zugeordnet sind (Anmerkungen)
        public List<Comment> Comments
        {
            get; set;
        }
    }
}
