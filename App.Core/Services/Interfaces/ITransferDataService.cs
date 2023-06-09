﻿using App.Core.DTOs;
using App.Core.Models;

namespace App.Core.Services.Interfaces
{
    public interface ITransferDataService<T> : ICrudService<T> where T : Transfer
    {
        public Task<Response<List<T>>> GetTransfersByPcb(int pcbId);
        public Task<Response<T>> CreateTransfer(T transfer, int? diagnoseId = null);
        public Task<Response<List<T>>> GetAllEager();
        public Task<Response<List<DwellTimeEvaluationDTO>>> GetAvgDwellTimeByStorageLocation(DateTime? from, DateTime? to);

    }
}
