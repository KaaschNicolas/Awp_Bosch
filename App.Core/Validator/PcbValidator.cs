using App.Core.Models;

namespace App.Core.Validator;

public class PcbValidator
{
    private Pcb _pcb;

    // Der Konstruktor der Klasse, der die zu überprüfende Leiterplatte erhält
    public PcbValidator(Pcb pcb)
    {
        _pcb = pcb;
    }

    private List<string> Validate()
    {
        List<string> errors = new List<string>();

        if (_pcb == null)
        {
            errors.Add("Pcb darf nicht null sein");
            return errors;
        }

        // Überprüfung, ob die Seriennummner der Leiterplaate null, leer oder länger als 10 Zeichen ist
        if (string.IsNullOrEmpty(_pcb.SerialNumber) || _pcb.SerialNumber.Length < 10)
        {
            errors.Add("SerialNumber muss 10 stellig sein.");
        }

        // Überprüfung auf ungültige ErrorTypes
        if (_pcb.ErrorTypes != null)
        {
            foreach (var errorType in _pcb.ErrorTypes)
            {
                if (!Enum.IsDefined(typeof(ErrorType), errorType))
                {
                    errors.Add($"Ungültiger ErrorType: {errorType}");
                }
            }
        }
        else
        {
            errors.Add("ErrorTypes is null.");
        }

        // Überprüfung auf ungültige PcbType
        /*if (!Enum.IsDefined(typeof(PcbType), _pcb.PcbType))
        {
            errors.Add($"Ungültiger PcbType: {_pcb.PcbType}");
        }*/

        // Überprüfung auf Enddiagnose bei Finalized
        if (_pcb.Finalized && _pcb.Diagnose == null)
        {
            errors.Add("Enddiagnose darf nicht null sein, wenn die Leiterplatte abgeschlossen ist");
        }

        // Überprüfung auf EndgültigerVerbleibOrt bei Finalized
        if (_pcb.Finalized && _pcb.Transfers?.LastOrDefault()?.StorageLocation == null)
        {
            errors.Add("EndgültigerVerbleibOrt darf nicht null sein, wenn die Leiterplatte abgeschlossen ist");
        }

        return errors; // Rückgabe der Liste der Validierungsfehler
    }

    // Eine Methode zur Überprüfung der Validität der Leiterplatte
    public bool IsValid()
    {
        // Überprüfung, ob die Anzahl der Validierungsfehler null ist
        if (Validate().Count == 0)
        {
            return true; // Die Leiterplatte ist valide
        }

        return false; // Es sind Validierungsfehler vorhanden
    }

    // Eine Methode zur Rückgabe der Liste der Validierungsfehler
    public List<string> GetErrors()
    {
        return Validate();
    }

}