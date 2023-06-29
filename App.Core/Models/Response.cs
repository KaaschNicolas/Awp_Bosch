using App.Core.Models.Enums;

namespace App.Core.Models;
public class Response<T>
{
    public ResponseCode Code { get; set; }

    public T Data { get; set; }

    public string ErrorMessage { get; set; }

    public Response(ResponseCode code, T data)
    {
        Code = code;
        Data = data;
    }

    public Response(ResponseCode code, string error)
    {
        Code = code;
        ErrorMessage = error;
    }

    public Response(ResponseCode code)
    {
        Code = code;
    }

    public Response() { }
}
