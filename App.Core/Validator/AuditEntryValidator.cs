using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;

namespace App.Core.Validator;
public class AuditEntryValidator
{
    private AuditEntry _auditEntry;
    public AuditEntryValidator(AuditEntry auditEntry)
    {
        _auditEntry = auditEntry;
    }
    private List<string> Validate()
    {
        List<string> errors = new List<string>();
        
        if(string.IsNullOrEmpty(_auditEntry.Message) || _auditEntry.Message.Length > 250)
        {
            errors.Add("Nachricht darf nicht leer sein oder 250 Zeichen überschreiten.");
        }
        if(_auditEntry.User == null)
        {
            errors.Add("Nutzer darf nicht null sein.");
        }
        if (_auditEntry.Level.Length > 20)
        {
            errors.Add("Level darf nicht 20 Zeichen überschreiten.");
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
