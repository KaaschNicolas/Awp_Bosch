namespace App.Core.Models
{
    // Die abstrakte Klasse "BaseEntity" dient als Basisklasse für andere Modelklassen
    public abstract class BaseEntity
    {
        // Die eindeutige ID des Entitätsobjekts
        public int Id { get; set; }
        // Das Erstellungsdatum des Entitätsobjekts
        public DateTime CreatedDate
        {
            get; set;
        }
        // Das Löschdatum des Entitätsobjekts
        public DateTime DeletedDate
        {
            get; set;
        }
    }
}
