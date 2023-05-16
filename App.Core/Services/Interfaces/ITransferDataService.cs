using App.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Services.Interfaces
{
    public interface ITransferDataService<T> : ICrudService<T> where T : Transfer
    {
        public Task<Response<List<T>>> GetTransfersByPcb(int pcbId);
    }
}
