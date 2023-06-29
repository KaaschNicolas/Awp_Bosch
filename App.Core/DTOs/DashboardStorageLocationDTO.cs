using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.DTOs
{
    // Das Data Transfer Object (DTO) "DashboardStorageLocationDTO" repräsentiert ein Objekt für das Dashboard mit Informationen zu einem Lagerort.
    // Es handelt sich um eine Keyless-Entität, da sie nicht über einen primären Schlüssel verfügt.
    [Keyless]
    public class DashboardStorageLocationDTO
    {
        // Name des Lagerorts
        public string StorageName { get; set; }
        // Anzahl der PCBs
        public int CountPcbs { get; set; }
        // Anzahl der PCBs mit grünem Status
        public int CountGreen { get; set; }
        // Anzahl der PCBs mit gelbem Status
        public int CountYellow { get; set; }
        // Anzahl der PCBs mit rotem Status
        public int CountRed { get; set; }

        // Anzahl der PCBs mit rotem Status
        [NotMapped]
        public int Percentage { get; set; }

        // Wert
        [NotMapped]
        public string Number { get; set; }
    }
}
