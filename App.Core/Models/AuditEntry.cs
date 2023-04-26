using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models;
public class AuditEntry : BaseEntity
{
    [MaxLength(250)]
    public string Message { get; set; }

    [NotMapped]
    [Required]
    public User User { get; set; }

    [MaxLength(20)]
    public string Level { get; set; }
    
    public string Exception { get; set; }
}
