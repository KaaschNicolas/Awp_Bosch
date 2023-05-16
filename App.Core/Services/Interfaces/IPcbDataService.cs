using App.Core.Models;

namespace App.Core.Services.Interfaces
{
    public interface IPcbDataService<T> : ICrudService<T> where T : Pcb
    {
    }
}
