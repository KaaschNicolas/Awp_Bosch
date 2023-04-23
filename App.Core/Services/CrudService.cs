using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public List<Umbuchung> GetUmbuchungen()
        {
            try
            {
                _boschContext.Umbuchungen.ToList();
            }
            catch (DbUpdateException)
            {

                throw;
            }
            return null;
        }

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
                Update();
            }
            catch (DbUpdateException ex)
            {

                throw;
            }
            return null;
        }

        public void Update()
        {
            try
            {
                _boschContext.SaveChanges();
            }
            catch (DbUpdateException)
            {

                throw;
            }
        }

        public List<Fehlertyp> GetErrorType()
        {
            try
            {
                _boschContext.Fehlertypen.ToList();
            }
            catch (DbUpdateException)
            {

                throw;
            }
            return null;
        }

        public void CreateErrorType(Fehlertyp errorType)
        {
            try
            {
                _boschContext.Fehlertypen.Add(errorType);
                Update();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
