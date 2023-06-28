using System.Reflection;

namespace App.Core.Helpers;
public static class PropertyHelper
{
    // Erweiterungsmethode, die die Eigenschaften eines Objekts in einen String konvertiert
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
