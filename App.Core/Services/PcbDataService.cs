using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Services.Base;
using App.Core.Services.Interfaces;

namespace App.Core.Services;
public class PcbDataService<T> : CrudServiceBase<T>, ICrudService<T>where T : Pcb
{
    public PcbDataService(BoschContext boschContext, LoggingService loggingService) : base(boschContext, loggingService) { }


}
