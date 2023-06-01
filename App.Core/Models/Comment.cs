namespace App.Core.Models
{
    public class Comment : BaseEntity
    {

        [Required]
        [Column(TypeName = "nvarchar(650)")]
        public string Content
        {
            get; set;
        }
        [Required]
        public User NotedBy
        {
            get; set;
        }

        public int NotedById
        {
            get; set;
        }

        public Pcb Pcb
        {
            get; set;
        }
    }
}
