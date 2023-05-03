using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;

namespace App.Core.Validator;
public class PcbTypeValidator
{
    private PcbType _pcbType;

    public PcbTypeValidator(PcbType pcbType)
    {
        _pcbType = pcbType;
    }

    private List<string> Validate()
    {
        List<string> errors = new List<string>();

        if(string.IsNullOrEmpty(_pcbType.PcbPartNumber) || _pcbType.PcbPartNumber.Length > 10)
        {
            errors.Add("Name darf nicht null sein oder 10 Zeichen überschreiten.");
        }

        if(_pcbType.Description == null)
        {
            errors.Add("Description darf nicht null sein.");
        }

        if (_pcbType.MaxTransfer <= 0)
        {
            errors.Add("MaxTransfer muss mindestens 1 betragen.");
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
