using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.DataAccess;
using App.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace App.Core.Services.Base;
public class CrudBase<T> where T : BaseEntity
{
    private BoschContext _boschContext;
    public CrudBase(BoschContext boschContext)
    {
        _boschContext = boschContext;
    }

    public async Task<Response> Create(T entity)
    {
        try
        {
            EntityEntry entityEntry = await _boschContext.Set<T>().AddAsync(entity);
            await _boschContext.SaveChangesAsync();
            return new Response(ResponseCode.Success, entityEntry);
        }
        catch (DbUpdateException ex)
        {
            return new Response(ResponseCode.Error, $"Fehler beim Erstellen von {nameof(T)} {ex.Source}");
        }
    }

    public async Task<Response> Update(int id, T entity)
    {
        try
        {
            entity.Id
        }
        catch (DbUpdateException ex)
        {

            throw;
        }
    }
}
