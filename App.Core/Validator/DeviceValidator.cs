using App.Core.Models;

namespace App.Core.Validator
{
    // Eine Klasse zur Validierung von Device-Objekten (Gerät - EInschränkungen)
    public class DeviceValidator
    {
        private Device _device;

        // Der Konstruktor der Klasse, der das zu überprüfende Device erhält 
        public DeviceValidator(Device device)
        {
            _device = device;
        }

        private List<string> Validate()
        {
            List<string> errors = new List<string>();

            // Überprüfung, ob der Name des Geräts null oder länger als 50 Zeichen ist
            if (string.IsNullOrEmpty(_device.Name) || _device.Name.Length > 50)
            {
                errors.Add("Name darf nicht null sein oder 50 Zeichen überschreiten.");
            }

            return errors; // Rückgabe der Liste der Validierungsfehler
        }

        // Eine Methode zur Überprüfung der Validität des Geräts
        public bool IsValid()
        {
            // Überprüfung, ob die Anzahl der Validierungsfehler 0 ist
            if (Validate().Count == 0)
            {
                return true; // Das Gerät ist valide
            }

            return false; // Es sind Validierungsfehler vorhanden
        }

        // Eine entliche Methode zur Rückgabe der Liste der Validierungsfehler
        public List<string> GetErrors()
        {
            return Validate();
        }
    }
}

