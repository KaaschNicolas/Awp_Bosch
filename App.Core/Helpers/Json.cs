using Newtonsoft.Json;

namespace App.Core.Helpers;

public static class Json
{

    // Asynchrone Methode zum Deserialisieren eines JSON-Strings zu einem Objekt vom angegebenen Typ
    public static async Task<T> ToObjectAsync<T>(string value)
    {
        return await Task.Run<T>(() =>
        {
            return JsonConvert.DeserializeObject<T>(value);
        });
    }

    // Asynchrone Methode zum Serialisieren eines Objekts zu einem JSON-String
    public static async Task<string> StringifyAsync(object value)
    {
        return await Task.Run<string>(() =>
        {
            return JsonConvert.SerializeObject(value);
        });
    }
}
