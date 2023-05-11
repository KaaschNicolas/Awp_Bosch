using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Services.Base;
using App.Core.Services.Interfaces;

namespace App.Core.Services;
public class PcbDataService<T> : CrudServiceBase<T>, ICrudService<T> where T : Pcb
{

    private readonly ICrudService<Transfer> _transferCrudService;
    private readonly ICrudService<ErrorType> _errorTypeCrudService;
    private readonly ICrudService<Device> _deviceCrudService;
    private readonly ICrudService<Comment> _commentCrudService;

    private Pcb _pcb;

    public PcbDataService(BoschContext boschContext, LoggingService loggingService) : base(boschContext, loggingService)
    {
        _boschContext = boschContext;

    }

    public async Task<Response<T>> Create(Transfer transfer, T entity, List<ErrorType> errorTypes, Device device, Comment comment)
    {

        try
        {
            using var transaction = _boschContext.Database.BeginTransaction();
            var deviceResponse = await _deviceCrudService.Create(device);
            var commentResponse = await _commentCrudService.Create(comment);

            entity.Restriction = deviceResponse.Data;
            entity.Comment = commentResponse.Data;

            var pcbEntity = await _boschContext.Set<T>().AddAsync(entity);

            transfer.Pcb = pcbEntity.Entity;
            var transferResponse = await _transferCrudService.Create(transfer);

            errorTypes[0].Pcb = pcbEntity.Entity;
            errorTypes[1].Pcb = pcbEntity.Entity;
            await _errorTypeCrudService.Create(errorTypes[0]);
            await _errorTypeCrudService.Create(errorTypes[1]);
            transaction.Commit();
            _pcb = pcbEntity.Entity;
        }
        catch (Exception)
        {
            return new Response<T>(ResponseCode.Error, (T)_pcb);
        }
        return new Response<T>(ResponseCode.Success, (T)_pcb);



    }

}
