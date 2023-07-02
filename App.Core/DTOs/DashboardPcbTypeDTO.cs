using System.ComponentModel.DataAnnotations.Schema;

namespace App.Core.DTOs
{
    // Das Data Transfer Object (DTO) "DashboardPcbTypeDTO" repräsentiert ein Objekt für das Dashboard mit Informationen zu einem PCB-Typ.
    // Die Klasse enthält Eigenschaften zur Anzahl der PCBs, zur PCB-Sachnummer sowie zu zusätzlichen Informationen für das Dashboard, wie den Wert und Prozentualen Anteil.
    public class DashboardPcbTypeDTO
    {
        // Anzahl der PCBs
        public int Count { get; set; }
        // Sachnummer des PCBs
        public string PcbPartNumber { get; set; }

        // Wert
        [NotMapped]
        public string Number { get; set; }

        // Prozentualer Anteil
        [NotMapped]
        public int Percentage { get; set; }
    }
}
