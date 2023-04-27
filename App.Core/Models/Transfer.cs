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
        public string Comment
        {
            get; set;
        }
        [Required]
        public StorageLocation StorageLocation
        {
            get; set;
        }
        [Required]
        public User NotedBy
        {
            get; set;
        }
        public Pcb Pcb
        {
            get; set;
        }
    }
}
