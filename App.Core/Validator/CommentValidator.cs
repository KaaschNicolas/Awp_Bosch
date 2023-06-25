using App.Core.Models;

namespace App.Core.Validator
{
    // Eine Klasse zur Validierung von Comment-Objekten (Anmerkung)
    public class CommentValidator
    {
        private Comment _comment;

        // Der Konstruktor der Klasse, der die zu überprüfende Comment erhält
        public CommentValidator(Comment comment)
        {
            _comment = comment;
        }

        private List<string> Validate()
        {
            List<string> errors = new List<string>();

            // Überprüfung, ob der Inhalt des Comments null ist oder mehr als 650 Zeichen enthält
            if (string.IsNullOrEmpty(_comment.Content) || _comment.Content.Length > 650)
            {
                errors.Add("Name darf nicht null sein oder 650 Zeichen überschreiten.");
            }

            // Überprüfung, ob der Verfasser des Comments null ist
            if (_comment.NotedBy == null)
            {
                errors.Add("NotedBy darf nicht null sein.");
            }

            return errors; // Rückgabe der Liste der Validierungsfehler
        }

        // Eine Methode zur Überprüfung der Validität des Comments
        public bool IsValid()
        {
            // Überprüfung, ob die Anzahl der Validierungsfehler 0 ist
            if (Validate().Count == 0)
            {
                return true; // Der Comment ist valide
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
