using App.Core.Models;

namespace App.Core.Validator
{
    // Eine Klasse zur Validierung von StorageLocation-Objekten
    public class StorageLocationValidator
    {
        private StorageLocation _storageLocation; // Eine private Instanzvariable zur Speicherung der zu überprüfenden StorageLocation

        // Der Konstruktor der Klasse, der die zu überprüfende StorageLocation erhält
        public StorageLocationValidator(StorageLocation storageLocation)
        {
            _storageLocation = storageLocation;
        }

        private List<string> Validate()
        {
            List<string> errors = new List<string>(); 

            // Überprüfung, ob der StorageName der StorageLocation null oder länger als 50 Zeichen ist
            if (string.IsNullOrEmpty(_storageLocation.StorageName) || _storageLocation.StorageName.Length > 50)
            {
                errors.Add("Code darf nicht null sein oder 50 Zeichen überschreiten.");
            }

            return errors; // Rückgabe der Liste der Validierungsfehler
        }

        // Eine Methode zur Überprüfung der Validität der StorageLocation
        public bool IsValid()
        {
            // Überprüfung, ob die Anzahl der Validierungsfehler null ist
            if (Validate().Count == 0)
            {
                return true; // Die StorageLocation ist valide
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
