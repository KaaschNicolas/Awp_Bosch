using App.Core.Models;

namespace App.Core.Validator;

public class PcbValidator
{
    /*private string _serialNumber;
    private Device _restriction;
    private string _errorDescription;
    private List<ErrorType> _errorTypes;
    private bool _finalized;
    private PcbType _pcbType;
    private Comment _comment;
    private List<Transfer> _transfers;
    private Diagnose _diagnose;*/

    private Pcb _pcb;

    public PcbValidator(Pcb pcb)
    {
        _pcb = pcb;
        /*_serialNumber = pcb.SerialNumber;
        _restriction = pcb.Restriction;
        _errorDescription = pcb.ErrorDescription;
        _errorTypes = pcb.ErrorTypes;
        _finalized = pcb.Finalized;
        _pcbType = pcb.PcbType;
        _comment = pcb.Comment;
        _transfers = pcb.Transfers;
        _diagnose = pcb.Diagnose;*/
    }

    private List<string> Validate()
    {
        List<string> errors = new List<string>();

        if (_pcb == null)
        {
            errors.Add("Pcb darf nicht null sein");
            return errors;
        }

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

        return errors;
    }
    public bool IsValid()
    {
        if (Validate().Count == 0)
        {
            return true;
        }

        return false;
    }

    public List<string> GetErrors()
    {
        return Validate();
    }

}