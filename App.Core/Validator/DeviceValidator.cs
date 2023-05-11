using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;

namespace App.Core.Validator;
public class DeviceValidator
{
    private Device _device;
    public DeviceValidator(Device device)
    {
        _device = device;
    }
    private List<string> Validate()
    {
        List<string> errors = new List<string>();

        if(string.IsNullOrEmpty(_device.Name) || _device.Name.Length > 50)
        {
            errors.Add("Name darf nicht null sein oder 50 Zeichen überschreiten.");
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
