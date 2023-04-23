using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Helpers;
public static class PropertyHelper
{
    public static string PropertiesToString(this object obj)
    {
        Type type = obj.GetType();
        PropertyInfo[] props = type.GetProperties();
        var str = "{";
        foreach (var prop in props)
        {
            str += (prop.Name + ":" + prop.GetValue(obj)) + ",";
        }
        return str.Remove(str.Length - 1) + "}";
    }
}
