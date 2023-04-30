using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;

namespace App.Core.Services.Interfaces;
public interface IStorageLocationDataService<T> where T : StorageLocation
{
    public IQueryable<T> GetAllQueryable();
}
