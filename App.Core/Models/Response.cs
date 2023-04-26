using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models;
public class Response
{
    public ResponseCode Code { get; set; }

    public object Data { get; set; }

    public Response(ResponseCode code, object data)
    {
        Code = code;
        Data = data;
    }
}
