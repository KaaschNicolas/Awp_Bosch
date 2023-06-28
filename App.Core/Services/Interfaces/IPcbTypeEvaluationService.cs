using App.Core.DTOs;
using App.Core.Models;
using App.Core.Services.Base;
using System.Linq.Expressions;

namespace App.Core.Services.Interfaces
{
    public interface IPcbTypeEvaluationService
    {
        public Task<Response<List<EvaluationStorageLocationDTO>>> GetAllByPcbType(string pcbType, DateTime deadline);

        public Task<Response<List<EvaluationFinalizedDTO>>> GetFinalizedByPcbType(string pcbType, DateTime deadline);

        public Task<Response<List<Dictionary<string, object>>>> GetPcbTypePosition(List<string> pcbTypeList, DateTime start, DateTime end);

        //public Task<Response<List<EvaluationPcbTypeI_ODTO>>> GetPcbTypeI_O(string pcbType, DateTime start, DateTime end);
    }
}
