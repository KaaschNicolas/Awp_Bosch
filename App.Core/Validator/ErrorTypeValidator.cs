using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;

namespace App.Core.Validator;
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
        if(string.IsNullOrEmpty(_errorType.Code) || _errorType.Code.Length > 5)
        {
            errors.Add("Code darf nicht null sein oder 5 Zeichen überschreiten.");
        }
        if(string.IsNullOrEmpty(_errorType.ErrorDescription) || _errorType.ErrorDescription.Length > 650)
        {
            errors.Add("Description darf nicht null sein oder 650 Zeichen überschreiten.");
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
