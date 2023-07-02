using Microsoft.EntityFrameworkCore;

namespace App.Core.DTOs
{
    // Das Data Transfer Object (DTO) "DwellTimeEvaluationDTO" repräsentiert eine abgespeckte Version für die Auswertung von Verweildauertdaten an einem Lagerort.
    // Es handelt sich um eine Keyless-Entität, da sie nicht über einen primären Schlüssel verfügt.
    [Keyless]
    public class DwellTimeEvaluationDTO
    {
        // Name des Lagerorts
        public string StorageName { get; set; }
        // Durchschnittliche Verweildauer
        public double AvgDwellTime { get; set; }
    }
}
