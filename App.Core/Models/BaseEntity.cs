namespace App.Core.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate
        {
            get; set;
        }
        public DateTime DeletedDate
        {
            get; set;
        }
    }
}
