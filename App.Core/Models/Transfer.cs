using System.ComponentModel.DataAnnotations;

namespace App.Core.Models
{
    public class Transfer : BaseEntity
    {

        public string Comment

        {
            get; set;
        }

        [Required]
        public int StorageLocationId { get; set; }

        [Required]
        public StorageLocation StorageLocation
        {
            get; set;
        }


        [Required]
        public int NotedById { get; set; }


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
