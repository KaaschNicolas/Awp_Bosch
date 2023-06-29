using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace App.Core.Models
{
    // Die Klasse "Comment" erbt von der Basisklasse "BaseEntity"
    public class Comment : BaseEntity
    {

        // Der Inhalt des Kommentars
        [Required]
        [Column(TypeName = "nvarchar(650)")]
        public string Content
        {
            get; set;
        }
        // Der Benutzer, der den Kommentar verfasst hat
        [Required]
        public User NotedBy
        {
            get; set;
        }

        // Die ID des Benutzers, der den Kommentar verfasst hat
        public int NotedById
        {
            get; set;
        }

        // Das PCB, dem dieser Kommentar zugeordnet ist
        public Pcb Pcb
        {
            get; set;
        }
    }
}
