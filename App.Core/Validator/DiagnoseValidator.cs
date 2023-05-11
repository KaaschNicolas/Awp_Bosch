using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;

namespace App.Core.Validator;
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
        if(string.IsNullOrEmpty(_diagnose.Name) || _diagnose.Name.Length > 650)
        {
            errors.Add("Name darf nicht null sein oder 650 Zeichen überschreiten.");
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
