namespace App.Core.Services;
public class PcbDataService<T> : CrudServiceBase<T>, IPcbDataService<T> where T : Pcb
{
    public PcbDataService(BoschContext boschContext, LoggingService loggingService) : base(boschContext, loggingService) { }

}