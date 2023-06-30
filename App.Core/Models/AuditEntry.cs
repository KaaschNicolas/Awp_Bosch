using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.Models;
public class AuditEntry : BaseEntity
{
    // Die Nachricht des Audit-Entry
    [MaxLength(250)]
    public string Message { get; set; }

    // Der Benutzer,bei dem der Audit-Entry erstellt wurde
    [NotMapped]
    [Required]
    public User User { get; set; }

    // Das Level des Audit-Entry
    [MaxLength(20)]
    public string Level { get; set; }

    // Die Ausnahme des Audit-Entry
    public string Exception { get; set; }
}
