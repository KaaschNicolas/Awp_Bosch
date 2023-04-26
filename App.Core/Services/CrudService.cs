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
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;

namespace App.Core.Services
{
    public class CrudService : ICrudService
    {
        public CrudService(BoschContext boschContext)
        {

        }

        private BoschContext _boschContext;

        private LoggingService _loggingService;

        public List<Pcb> GetCompleteLeiterplatten() => _boschContext.Pcbs.Include(x => x.Transfers).ToList();

        public List<StorageLocation> GetLagerorte() => _boschContext.StorageLocations.ToList();

        public List<PcbType> GetPcbType()
        {
            _loggingService.Audit(LogLevel.Debug, "GetPcbType");
            try
            {
                var query = _boschContext.PcbTypes.ToList();
                _loggingService.Audit(
                    LogLevel.Information,
                    "GetPcbType",
                    null,
                    null,
                    obj: query    
                    );
            }
            catch (DbUpdateException)
            {

                throw;
            }
            return null;
        }

        public List<User> GetUser()
        {
            _loggingService.Audit(LogLevel.Debug, "GetUser");
            try
            {
                _boschContext.Users.ToList();
            }
            catch (DbUpdateException)
            {

                throw;
            }
            return null;
        }

        public List<Transfer> GetTransfer()
        {
            _loggingService.Audit(LogLevel.Debug, "GetTransfer");
            try
            {
                _boschContext.Transfers.ToList();
            }
            catch (DbUpdateException)
            {

                throw;
            }
            return null;
        }

        public List<Transfer> GetTransfer(Pcb leiterplatte)
        {
            _loggingService.Audit(LogLevel.Debug, "GetTransfer");
            string test;
            try
            {
                var query = _boschContext.Transfers.Where(x => x.Id == leiterplatte.Id).ToList();

            }
            catch (DbUpdateException ex)
            {
                test = ex.Source;

            }
            //Platzhalter für unsere ResponseCodes
            return null;

        }

        public List<Pcb> CreatePcb(Pcb leiterplatte)
        {
            _loggingService.Audit(LogLevel.Debug, "CreatePcb");
            try
            {
                var query = _boschContext.Pcbs.Add(leiterplatte);
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
            _loggingService.Audit(LogLevel.Debug, "Update");
            try
            {
                _boschContext.SaveChanges();
            }
            catch (DbUpdateException)
            {

                throw;
            }
        }

        public List<ErrorType> GetErrorType()
        {
            _loggingService.Audit(LogLevel.Debug, "GetErrorType");
            try
            {
                _boschContext.ErrorTypes.ToList();
            }
            catch (DbUpdateException)
            {

                throw;
            }
            return null;
        }

        public void CreateErrorType(ErrorType errorType)
        {
            _loggingService.Audit(LogLevel.Debug, "CreateErrorType");
            try
            {
                _boschContext.ErrorTypes.Add(errorType);
                Update();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<StorageLocation>> GetStorageLocation()
        {
            _loggingService.Audit(LogLevel.Debug, "GetStorageLocation");
            try
            {
                await _boschContext.StorageLocations.ToListAsync();
            }
            catch (DbUpdateException)
            {

                throw;
            }
            return null;
        }

        public Task<List<StorageLocation>> GetStorageLocationByPcb(Pcb pcb)
        {
            _loggingService.Audit(LogLevel.Debug, "GetStorageLocationByPcb");
            try
            {
                //_boschContext.LagerOrte.Where(x => x.Umbuchungen.ForEach(e => e == pcb.Weitergaben));
            }
            catch (DbUpdateException)
            {

                throw;
            }
            return null;
        }
    }
}
