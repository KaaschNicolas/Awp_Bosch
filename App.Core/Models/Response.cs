using App.Core.Models.Enums;

namespace App.Core.Models;
// Die generische Klasse "Response<T>" repräsentiert eine Antwort auf eine Operation oder einen Vorgang
public class Response<T>
{
    // Der Rückgabecode der Antwort
    public ResponseCode Code { get; set; }

    // Die Daten, die in der Antwort enthalten sind
    public T Data { get; set; }

    // Die Fehlermeldung, die in der Antwort enthalten ist
    public string ErrorMessage { get; set; }

    // Konstruktor, der den Rückgabecode und die Daten für die Antwort festlegt
    public Response(ResponseCode code, T data)
    {
        Code = code;
        Data = data;
    }

    // Konstruktor, der den Rückgabecode und die Fehlermeldung für die Antwort festlegt
    public Response(ResponseCode code, string error)
    {
        Code = code;
        ErrorMessage = error;
    }

    // Konstruktor, der nur den Rückgabecode für die Antwort festlegt
    public Response(ResponseCode code)
    {
        Code = code;
    }

    // Standardkonstruktor
    public Response() { }
}
