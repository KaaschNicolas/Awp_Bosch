using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;

namespace App.Core.Validator;
public class StorageLocationValidator
{
    private StorageLocation _storageLocation;

    public StorageLocationValidator(StorageLocation storageLocation)
    {
        _storageLocation = storageLocation;
    }

private List<string> Validate()
{
    List<string> errors = new List<string>();
    if (string.IsNullOrEmpty(_storageLocation.StorageName) || _storageLocation.StorageName.Length > 50)
    {
        errors.Add("Code darf nicht null sein oder 50 Zeichen überschreiten.");
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