using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Services.Base;
using App.Core.Services.Interfaces;

namespace App.Core.Services
{
    public class CrudService<T> : CrudServiceBase<T>, ICrudService<T> where T : BaseEntity
    {
        public CrudService(BoschContext boschContext, ILoggingService loggingService) : base(boschContext, loggingService)
        {

        }

    }
}
