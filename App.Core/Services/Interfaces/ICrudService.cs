using App.Core.Models;
using App.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Services.Interfaces
{
    public interface ICrudService
    {
<<<<<<< HEAD
        public List<Leiterplatte> GetCompleteLeiterplatten();
        public Task<Leiterplattentyp> Create(Leiterplattentyp leiterplattentyp);
=======
        public List<Pcb> GetCompleteLeiterplatten();
>>>>>>> master
    }
}
