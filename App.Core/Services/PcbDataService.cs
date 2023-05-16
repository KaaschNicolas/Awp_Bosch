using App.Core.DataAccess;
using App.Core.Models;
using App.Core.Services;
using App.Core.Services.Base;
using App.Core.Services.Interfaces;
public class PcbDataService<T> : CrudServiceBase<T>, IPcbDataService<T> where T : Pcb
{
    public PcbDataService(BoschContext boschContext, LoggingService loggingService) : base(boschContext, loggingService) { }

}