using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Services.ResponseHandler.impl;

public class ResponseHandler<T> : IResponseHandler<T> where T : DbContext
{
    private readonly DbContext _dbContext;

    public ResponseHandler(T dbContext)
    {
        this._dbContext = dbContext;
    }

    public Response<int> ExecuteNonQuery(string commandText, params object[] parameters)
    {
        try
        {

            return new Response<int>(ResponseCode.Success, "Query executed successfully", _dbContext.Database.ExecuteSqlRaw(commandText, parameters));
        }
        catch (Exception ex)
        {
            return new Response<int>(ResponseCode.Error, "Error executing query: " + ex.Message);
        }
    }

    public Response<bool> CheckConnection()
    {
        try
        {
            _dbContext.Database.CanConnect();
            return new Response<bool>(ResponseCode.Success, "Connection successful", true);
        }
        catch (Exception ex)
        {
            return new Response<bool>(ResponseCode.Error, "Error connecting to database: " + ex.Message, false);
        }
    }

    public Response<bool> ExecuteTransaction(Action<DbContext> action)
    {
        using (var transaction = _dbContext.Database.BeginTransaction())
        {
            try
            {
                action(_dbContext);
                transaction.Commit();
                return new Response<bool>(ResponseCode.Success, "Transaction executed successfully", true);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new Response<bool>(ResponseCode.Error, "Error executing transaction: " + ex.Message, false);
            }
        }
    }
}

public class Response<T>
{
    public ResponseCode Code
    {
        get; set;
    }
    public string Description
    {
        get; set;
    }
    public T Data
    {
        get; set;
    }

    public Response(ResponseCode code, string description)
    {
        Code = code;
        Description = description;
    }

    public Response(ResponseCode code, string description, T data)
    {
        Code = code;
        Description = description;
        Data = data;
    }
}

public enum ResponseCode
{
    Success,
    Error
}