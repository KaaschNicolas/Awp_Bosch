namespace App.Core.Services.Interfaces
{
    public interface ITransferDataService<T> : ICrudService<T> where T : Transfer
    {
        public Task<Response<List<T>>> GetTransfersByPcb(int pcbId);
        public Task<Response<T>> CreateTransfer(T transfer, int diagnoseId);

    }
}
