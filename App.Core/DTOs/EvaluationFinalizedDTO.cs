using Microsoft.EntityFrameworkCore;

namespace App.Core.DTOs
{
    // Das Data Transfer Object (DTO) "EvaluationFinalizedDTO" repräsentiert ein Objekt für die Auswertung von abgeschlossenen PCBs.
    // Es handelt sich um eine Keyless-Entität, da sie nicht über einen primären Schlüssel verfügt.
    [Keyless]
    public class EvaluationFinalizedDTO
    {
        // Gesamtzahl der PCBs in Bearbeitung
        public int TotalInProgress { get; set; }

        // Gesamtzahl der abgeschlossenen PCBs
        public int TotalFinalized { get; set; }
    }
}
