﻿using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Services.Base;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Services
{
    public class TransferDataService<T> : CrudServiceBase<T>, ITransferDataService<T> where T : Transfer
    {
        public TransferDataService(BoschContext boschContext, ILoggingService loggingService) : base(boschContext, loggingService) { }

        public async Task<Response<List<T>>> GetTransfersByPcb(int pcbId)
        {
            try
            {
                var data = await _boschContext
                    .Set<T>()
                    .Where(x => x.Id == pcbId)
                    .Include("Pcb")
                    .Include("StorageLocation")
                    .Include("NotedBy")
                    .ToListAsync();
                return new Response<List<T>>(ResponseCode.Success, data: data);
            }
            catch (DbUpdateException)
            {
                return new Response<List<T>>(ResponseCode.Error, error: "GetTransfersByPcb() failed");
            }
        }


        public async Task<ResponseCode> CreateTransfer(Transfer transfer, int diagnoseId)
        {
            using (var transaction = await _boschContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _boschContext.Set<Transfer>().AddAsync(transfer);
                    var pcb = await _boschContext.Set<Pcb>().FirstOrDefaultAsync(x => x.Id == transfer.PcbId);
                    pcb.DiagnoseId = diagnoseId;

                    await _boschContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                }
                catch (Exception)
                {
                    return ResponseCode.Error;
                }
                return ResponseCode.Success;
            }
        }
    }
}
