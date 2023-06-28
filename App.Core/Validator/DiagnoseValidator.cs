using App.Core.Models;

namespace App.Core.Validator
{
    // Eine Klasse zur Validierung von Diagnose-Objekten (Fehlerkategorie)
    public class DiagnoseValidator
    {
        private Diagnose _diagnose;
        public DiagnoseValidator(Diagnose diagnose)
        {
            _diagnose = diagnose;
        }

        private List<string> Validate()
        {
            List<string> errors = new List<string>();

            // Überprüfung, ob der Name der Diagnose null oder länger als 650 Zeichen ist
            if (string.IsNullOrEmpty(_diagnose.Name) || _diagnose.Name.Length > 650)
            {
                errors.Add("Name darf nicht null sein oder 650 Zeichen überschreiten.");
            }

            return errors; // Rückgabe der Liste der Validierungsfehler
        }

        // Eine Methode zur Überprüfung der Validität der Diagnose
        public bool IsValid()
        {
            // Überprüfung, ob die Anzahl der Validierungsfehler 0 ist
            if (Validate().Count == 0)
            {
                return true; // Die Diagnose ist valide
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

