using App.Core.Services.ResponseHandler.impl;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Services.ResponseHandler;
public interface IResponseHandler<T>
{
    Response<int> ExecuteNonQuery(string commandText, params object[] parameters);
    Response<bool> CheckConnection();
    Response<bool> ExecuteTransaction(Action<DbContext> action);
}
