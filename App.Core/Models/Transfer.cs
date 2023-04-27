using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class Transfer : BaseEntity
    {
        [Key]
        public int Id
        {
            get; set;
        }
        public string Anmerkung
        {
            get; set;
        }
        [Required]
        public StorageLocation Nach
        {
            get; set;
        }
        [Required]
        public User VerbuchtVon
        {
            get; set;
        }
        public Pcb Leiterplatte
        {
            get; set;
        }
    }
}
