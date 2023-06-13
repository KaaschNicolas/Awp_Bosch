using App.Core.Models;

namespace App.Core.Services.Interfaces
{
    public interface ICrudService<T> where T : BaseEntity
    {
        public Task<Response<T>> Create(T entity);
        public Task<Response<T>> Update(int id, T entity);
        public Task<Response<T>> Delete(T entity);
        public Task<Response<List<T>>> GetAll();
        public Task<Response<T>> GetById(int id);

        public Task Dispose();
    }
}
