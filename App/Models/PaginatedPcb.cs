using App.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class PaginatedPcb
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DeletedDate { get; set; }
        [MaxLength(10)]
        public string SerialNumber
        {
            get; set;
        }
        public Device Restriction
        {
            get; set;
        }
        public string ErrorDescription
        {
            get; set;
        }
        public List<ErrorType> ErrorTypes
        {
            get; set;
        }
        public bool Finalized
        {
            get; set;
        }
        [Required]
        public int PcbTypeId
        {
            get; set;
        }
        public PcbType PcbType
        {
            get; set;
        }
        public Comment Comment
        {
            get; set;
        }

        /// <summary>
        /// List of Transfers a pcb went through
        /// The List should always be orderd by ID or the field Created_At.
        /// The first Trasnfer is always the starting destination of the pcb
        /// The last Transfer is the End destination of the pcb but only if the pcb is finalized.
        /// /// </summary>
        public List<Transfer> Transfers
        {
            get; set;
        }

        public Diagnose? Diagnose
        {
            get; set;
        }

        public int? DiagnoseId
        {
            get; set;
        }
        public string? LastStorageLocationName { get; set; }


        public static PaginatedPcb ToPaginatedPcb(Pcb pcb)
        {
            string? currentStorageLocationName = pcb.Transfers.Count > 0 ? pcb.Transfers[0].StorageLocation.StorageName : null;
            return new PaginatedPcb()
            {
                Id = pcb.Id,
                CreatedDate = pcb.CreatedDate,
                DeletedDate = pcb.DeletedDate,
                SerialNumber = pcb.SerialNumber,
                Restriction = pcb.Restriction,
                ErrorDescription = pcb.ErrorDescription,
                ErrorTypes = pcb.ErrorTypes,
                Finalized = pcb.Finalized,
                PcbTypeId = pcb.PcbTypeId,
                PcbType = pcb.PcbType,
                Comment = pcb.Comment,
                Transfers = pcb.Transfers, // Hier ist jetzt halt nur einer drin weil wir uns nur den neusten zurück geben
                Diagnose = pcb.Diagnose,
                DiagnoseId = pcb.DiagnoseId,
                LastStorageLocationName = currentStorageLocationName,
            };
        }
        public static PaginatedPcb ToPaginatedPcb(Pcb pcb, Transfer transfer, List<StorageLocation> storageLocations, Func<Transfer, List<StorageLocation>, string> storageLocation)
        {
            return new PaginatedPcb()
            {
                Id = pcb.Id,
                CreatedDate = pcb.CreatedDate,
                DeletedDate = pcb.DeletedDate,
                SerialNumber = pcb.SerialNumber,
                Restriction = pcb.Restriction,
                ErrorDescription = pcb.ErrorDescription,
                ErrorTypes = pcb.ErrorTypes,
                Finalized = pcb.Finalized,
                PcbTypeId = pcb.PcbTypeId,
                PcbType = pcb.PcbType,
                Comment = pcb.Comment,
                Transfers = pcb.Transfers,
                Diagnose = pcb.Diagnose,
                DiagnoseId = pcb.DiagnoseId,
                LastStorageLocationName = storageLocation.Invoke(transfer, storageLocations)
            };
        }

        public static Pcb ToPcb(PaginatedPcb paginatedPcb)
        {
            return new Pcb()
            {
                Id = paginatedPcb.Id,
                CreatedDate = paginatedPcb.CreatedDate,
                DeletedDate = paginatedPcb.DeletedDate,
                SerialNumber = paginatedPcb.SerialNumber,
                Restriction = paginatedPcb.Restriction,
                ErrorDescription = paginatedPcb.ErrorDescription,
                ErrorTypes = paginatedPcb.ErrorTypes,
                Finalized = paginatedPcb.Finalized,
                PcbTypeId = paginatedPcb.PcbTypeId,
                PcbType = paginatedPcb.PcbType,
                Comment = paginatedPcb.Comment,
                Transfers = paginatedPcb.Transfers,
                Diagnose = paginatedPcb.Diagnose,
                DiagnoseId = paginatedPcb.DiagnoseId,
            };
        }
    }
}
