using App.Core.Models;

namespace App.Core.Validator
{
    public class ErrorTypeValidator
    {
        private ErrorType _errorType;
        public ErrorTypeValidator(ErrorType errorType)
        {
            _errorType = errorType;
        }

        private List<string> Validate()
        {
            List<string> errors = new List<string>();

            // Überprüfung, ob der Code des ErrorType null oder länger als 5 Zeichen ist
            if (string.IsNullOrEmpty(_errorType.Code) || _errorType.Code.Length > 5)
            {
                errors.Add("Code darf nicht null sein oder 5 Zeichen überschreiten.");
            }

            // Überprüfung, ob die ErrorDescription des ErrorType null oder länger als 650 Zeichen ist
            if (string.IsNullOrEmpty(_errorType.ErrorDescription) || _errorType.ErrorDescription.Length > 650)
            {
                errors.Add("Description darf nicht null sein oder 650 Zeichen überschreiten.");
            }

            return errors; 
        }

        // Eine Methode zur Überprüfung der Validität des ErrorType
        public bool IsValid()
        {
            // Überprüfung, ob die Anzahl der Validierungsfehler 0 ist
            if (Validate().Count == 0)
            {
                return true; // Der ErrorType ist valide
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

