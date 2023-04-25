using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Services.Interfaces;
using App.Core.Services.ResponseHandler;
using App.Core.Services.ResponseHandler.impl;
using Microsoft.EntityFrameworkCore;

namespace App.Core.Services
{
    public class CrudService : ICrudService
    {
        private IResponseHandler<DbContext> _responseHandler;
        private BoschContext _boschContext;
        public CrudService(BoschContext boschContext)
        {
            this._responseHandler = new ResponseHandler<DbContext>(boschContext);
            this._boschContext = boschContext;
        }

        public List<Leiterplatte> GetCompleteLeiterplatten() => _boschContext.Leiterplatten.Include(x => x.Weitergaben).ToList();

        public List<Leiterplatte> GetCompleteLeiterplattenExample()
        {
            var response = _responseHandler.ExecuteQuery<Leiterplatte>("Leiterplatten.Include(x => x.Weitergaben).ToList()");

            // Überprüfe die Antwort und die Ergebnisse
            if (response.Code == ResponseCode.Success)
            {
                return response.Data;
            }
            else
            {
                // Fehlerbehandlung einfügen..
                var errorMessage = response.Description;
                return null;
            }
        }


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

                var response = _responseHandler.AddEntity(leiterplatte);
                if (response.Code == ResponseCode.Success)
                {
                    //OK
                    return null;
                }
                else
                {
                    //NICHT OK
                    return null;
                }

                //var query = _boschContext.Leiterplatten.Add(leiterplatte);
                //_boschContext.SaveChanges();

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

    }
}
