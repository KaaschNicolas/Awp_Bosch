using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;

namespace App.Core.Validator;
public class TransferValidator
{
    private Transfer _transfer;
    public TransferValidator(Transfer transfer)
    {
        _transfer = transfer;
    }
    private List<string> Validate()
    {
        List<string> errors = new List<string>();
        if(_transfer.StorageLocation == null)
        {
            errors.Add("StorageLocation darf nicht null sein.");
        }
        if(_transfer.NotedBy == null)
        {
            errors.Add("NotedBy darf nicht null sein.");
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
