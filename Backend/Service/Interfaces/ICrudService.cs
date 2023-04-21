using Backend.DataAccess;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Service.Interfaces
{
    public interface ICrudService
    {
        public List<Leiterplatte>? GetCompleteLeiterplatten();
    }
}
