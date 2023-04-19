using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Backend.DataAccess;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Service
{
    public class CrudService
    {
        public CrudService(BoschContext boschContext)
        {

        }

        private BoschContext _boschContext;

        public List<Leiterplatte> GetCompleteLeiterplatten() => _boschContext.Leiterplatten.Include(x => x.Weitergaben).ToList();

        public List<LagerOrt> GetLagerorte() => _boschContext.LagerOrte.ToList();

        public List<Leiterplattentyp> GetLeiterplattentypen => _boschContext.Leiterplattentypen.ToList();

        public List<Nutzer> GetNutzer() => _boschContext.Nutzende.ToList();

        public List<Umbuchung>? GetUmbuchungen(Leiterplatte leiterplatte)
        {
            return leiterplatte?.Weitergaben != null ? leiterplatte.Weitergaben : null;
        }
        
    }
}
