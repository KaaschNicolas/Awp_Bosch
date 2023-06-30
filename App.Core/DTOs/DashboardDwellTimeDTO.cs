using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.DTOs
{
    // Das Data Transfer Object (DTO) "DashboardDwellTimeDTO" repräsentiert ein Objekt für das Dashboard mit Informationen zur Verweildauer.
    // Die Klasse enthält Eigenschaften zum Verweildauerstatus, zur Anzahl, sowie zu zusätzlichen Informationen für das Dashboard.
    [Keyless]
    public class DashboardDwellTimeDTO
    {
        // Verweildauerstatus
        public int DwellTimeStatus { get; set; }
        // Anzahl der Verweildauerstatus
        public int CountDwellTimeStatus { get; set; }

        // Farbe
        [NotMapped]
        public string Color { get; set; }

        // Prozentualer Anteil
        [NotMapped]
        public int Percentage { get; set; }
    }
}
