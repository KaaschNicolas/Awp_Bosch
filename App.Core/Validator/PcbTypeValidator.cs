using App.Core.Models;

namespace App.Core.Validator
{
    // Eine Klasse zur Validierung von PcbType-Objekten (ProgrammableCircuitBoardType - LeiterplattenSachnummer)
    public class PcbTypeValidator
    {
        private PcbType _pcbType; 

        // Der Konstruktor der Klasse, der den zu überprüfenden PcbType erhält
        public PcbTypeValidator(PcbType pcbType)
        {
            _pcbType = pcbType;
        }

        private List<string> Validate()
        {
            List<string> errors = new List<string>(); 

            // Überprüfung, ob die PcbPartNumber null oder länger als 10 Zeichen ist
            if (string.IsNullOrEmpty(_pcbType.PcbPartNumber) || _pcbType.PcbPartNumber.Length > 10)
            {
                errors.Add("Name darf nicht null sein oder 10 Zeichen überschreiten.");
            }

            // Überprüfung, ob die Description des PcbType null ist
            if (_pcbType.Description == null)
            {
                errors.Add("Description darf nicht null sein.");
            }

            // Überprüfung, ob der MaxTransfer-Wert des PcbType kleiner oder gleich 0 ist
            if (_pcbType.MaxTransfer <= 0)
            {
                errors.Add("MaxTransfer muss mindestens 1 betragen.");
            }

            return errors; // Rückgabe der Liste der Validierungsfehler
        }

        // Eine Methode zur Überprüfung der Validität des PcbType
        public bool IsValid()
        {
            // Überprüfung, ob die Anzahl der Validierungsfehler null ist
            if (Validate().Count == 0)
            {
                return true; // Der PcbType ist valide
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

