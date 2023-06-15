using App.Core.DTOs;
using App.Core.Models;
using App.Core.Services.Base;
using System.Linq.Expressions;

namespace App.Core.Services.Interfaces
{
    public interface IPcbTypeEvaluationService
    {
        public Task<Response<List<EvaluationStorageLocationDTO>>> GetAllByPcbType(string pcbType, DateTime deadline);


    }
}
