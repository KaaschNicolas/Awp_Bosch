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
using App.Core.Services.Base;

namespace App.Core.Services
{
    public class CrudService<T> : CrudServiceBase<T>, ICrudService<T> where T : BaseEntity
    {
        public CrudService(BoschContext boschContext, LoggingService loggingService) : base(boschContext, loggingService)
        {
            
        }
    }
}
