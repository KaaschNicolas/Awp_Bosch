using App.Core.Models;

namespace App.Core.Services.Interfaces
{
    public interface IPcbDataService<T> where T : Pcb
    {
        public Task<Response<List<T>>> Create(Transfer transfer, T entity, List<ErrorType> errorTypes, Device device, Comment comment);


    }

}
