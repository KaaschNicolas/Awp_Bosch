using App.Core.Models;

namespace App.Core.Validator
{
    // Eine Klasse zur Validierung von AuditEntry-Objekten (Logging Objekte)
    public class AuditEntryValidator
    {
        private AuditEntry _auditEntry;

        // Der Konstruktor der Klasse, der das zu überprüfende AuditEntry erhält
        public AuditEntryValidator(AuditEntry auditEntry)
        {
            _auditEntry = auditEntry;
        }

        private List<string> Validate()
        {
            List<string> errors = new List<string>();

            // Überprüfung, ob die Nachricht des AuditEntrys leer ist oder mehr als 250 Zeichen enthält
            if (string.IsNullOrEmpty(_auditEntry.Message) || _auditEntry.Message.Length > 250)
            {
                errors.Add("Nachricht darf nicht leer sein oder 250 Zeichen überschreiten.");
            }

            // Überprüfung, ob der Nutzer des AuditEntry null ist
            if (_auditEntry.User == null)
            {
                errors.Add("Nutzer darf nicht null sein.");
            }

            // Überprüfung, ob das Level des AuditEntrys mehr als 20 Zeichen enthält
            if (_auditEntry.Level.Length > 20)
            {
                errors.Add("Level darf nicht 20 Zeichen überschreiten.");
            }

            return errors; // Rückgabe der Liste der Validierungsfehler
        }

        // Eine Methode zur Überprüfung der Validität des AuditEntry
        public bool IsValid()
        {
            // Überprüfung, ob die Anzahl der Validierungsfehler 0 ist
            if (Validate().Count == 0)
            {
                return true; // Der AuditEntry ist valide
            }

            return false; // Es sind Validierungsfehler vorhanden
        }

        // Eine Methode zur Rückgabe der Liste der Validierungsfehler
        public List<string> GetErrors()
        {
            return Validate();
        }
    }
}

