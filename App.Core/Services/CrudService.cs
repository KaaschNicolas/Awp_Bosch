using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace App.Core.Services
{
    public class CrudService : ICrudService
    {
        public CrudService(BoschContext boschContext)
        {

        }

        private BoschContext _boschContext;

        public List<Leiterplatte> GetCompleteLeiterplatten() => _boschContext.Leiterplatten.Include(x => x.Weitergaben).ToList();

        public List<LagerOrt> GetLagerorte() => _boschContext.LagerOrte.ToList();

        public List<Leiterplattentyp> GetLeiterplattentypen => _boschContext.Leiterplattentypen.ToList();

        public List<Nutzer> GetNutzer() => _boschContext.Nutzende.ToList();

        public List<Umbuchung> GetUmbuchungen(Leiterplatte leiterplatte)
        {
            string test;
            try
            {
                var query = _boschContext.Umbuchungen.Where(x => x.Id == leiterplatte.Id).ToList();

            }
            catch (DbUpdateException ex)
            {
                test = ex.Source;

            }
            //Platzhalter für unsere ResponseCodes
            return null;

        }

        public List<Leiterplatte> CreateLeiterplatte(Leiterplatte leiterplatte)
        {
            try
            {
                var query = _boschContext.Leiterplatten.Add(leiterplatte);
                _boschContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {

                throw;
            }
            return null;
        }

        public List<Leiterplatte> UpdateLeiterplatte()
        {
            try
            {
                _boschContext.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }

        public async Task<Leiterplattentyp> Create(Leiterplattentyp leiterplattentyp)
        {
            EntityEntry<Leiterplattentyp> createdResult = await _boschContext.Leiterplattentypen.AddAsync(leiterplattentyp);
            await _boschContext.SaveChangesAsync();
            return createdResult.Entity;
        }



        

    }
}
