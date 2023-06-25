using App.Core.Models;

namespace App.Core.Validator
{
    // Eine Klasse zur Validierung von Transferobjekten
    public class TransferValidator
    {
        private Transfer _transfer;

        // Der Konstruktor der Klasse, der das zu überprüfende Transferobjekt erhält
        public TransferValidator(Transfer transfer)
        {
            _transfer = transfer;
        }
        // Validiert das Transferobjekt und gibt ggf. eine Liste an Validierungsfehlern zurück
        private List<string> Validate()
        {
            List<string> errors = new List<string>(); 

            // Überprüfung, ob die StorageLocation des Transferobjekts null ist
            if (_transfer.StorageLocation == null)
            {
                errors.Add("StorageLocation darf nicht null sein.");
            }

            // Überprüfung, ob die NotedBy-Eigenschaft des Transferobjekts null ist
            if (_transfer.NotedBy == null)
            {
                errors.Add("NotedBy darf nicht null sein.");
            }

            return errors;
        }

        // Eine Methode zur Überprüfung der Validität des Transferobjekts
        public bool IsValid()
        {
            // Überprüfung, ob die Anzahl der Validierungsfehler 0 ist
            if (Validate().Count == 0)
            {
                return true; // Das Transferobjekt ist valide
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
