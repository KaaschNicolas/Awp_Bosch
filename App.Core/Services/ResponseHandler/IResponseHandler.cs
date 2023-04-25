using App.Core.Services.ResponseHandler.impl;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Services.ResponseHandler;
public interface IResponseHandler<T>
{
    Response<int> ExecuteNonQuery(string commandText, params object[] parameters);
    Response<List<TEntity>> ExecuteQuery<TEntity>(string query, params object[] parameters) where TEntity : class;
    Response<bool> CheckConnection();
    Response<bool> ExecuteTransaction(Action<DbContext> action);
    Response<bool> Update<TEntity>(TEntity entity) where TEntity : class;
    public Response<bool> Remove<TEntity>(TEntity entity) where TEntity : class;
    Response<bool> AddEntity<TEntity>(TEntity entity) where TEntity : class;

}
