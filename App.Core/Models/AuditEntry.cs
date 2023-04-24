using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models;
public class AuditEntry : BasisEntitaet
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(250)]
    public string Message { get; set; }
    
    [Required]
    public Nutzer User { get; set; }

    [MaxLength(20)]
    public string Level { get; set; }
    
    public string Exception { get; set; }
}
