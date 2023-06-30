using Microsoft.EntityFrameworkCore;

namespace App.Core.DTOs
{
    // Das Data Transfer Object (DTO) "EvaluationStorageLocationDTO" repräsentiert eine abgespeckte Version der StorageLocation-Entität für die Auswertung.
    // Es handelt sich um eine Keyless-Entität, da sie nicht über einen primären Schlüssel verfügt.
    [Keyless]
    public class EvaluationStorageLocationDTO 
    {
        // Name des Speicherorts
        public string StorageName { get; set; }

        // Gesamtanzahl an PCB an diesem Lagerort
        public int SumCount { get; set; }

        // Anzahl vor dem Stichtag
        public int CountBefore { get; set; }

        // Anzahl nach dem Stichtag
        public int CountAfter { get; set; }
    }
}
