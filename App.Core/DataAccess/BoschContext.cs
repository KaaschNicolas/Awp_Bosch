using App.Core.DTOs;
using App.Core.Helpers;
using App.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App.Core.DataAccess
{
    // Die Klasse BoschContext erbt von DbContext, was eine Basisklasse für den Datenbankzugriff in Entity Framework ist.
    public class BoschContext : DbContext
    {
        // Standardkonstruktor
        public BoschContext()
        {
        }

        // Konstruktor mit DbContextOptions-Parameter, der von der Basisklasse aufgerufen wird
        public BoschContext(DbContextOptions<BoschContext> options) : base(options) { }

        // DbSet definiert eine Entität in der Datenbank. Hier sind verschiedene DbSet-Definitionen für Tabellen in der Datenbank.

        // Tabelle "Comments"
        public DbSet<Comment> Comments
        {
            get; set;
        }

        // Tabelle "Diagnoses"
        public DbSet<Diagnose> Diagnoses
        {
            get; set;
        }

        // Tabelle "ErrorTypes"
        public DbSet<ErrorType> ErrorTypes
        {
            get; set;
        }

        // Tabelle "Devices"
        public DbSet<Device> Devices
        {
            get; set;
        }

        // Tabelle "StorageLocations"
        public DbSet<StorageLocation> StorageLocations
        {
            get; set;
        }

        // Tabelle "Pcbs"
        public DbSet<Pcb> Pcbs
        {
            get; set;
        }

        // Virtuelles DbSet für "PcbDTO"
        public virtual DbSet<PcbDTO> PcbsDTO { get; set; }

        // Virtuelles DbSet für "DwellTimeEvaluationDTO"
        public virtual DbSet<DwellTimeEvaluationDTO> DwellTimeEvaluationDTO { get; set; }

        // Virtuelles DbSet für "DashboardStorageLocationDTO"
        public virtual DbSet<DashboardStorageLocationDTO> DashboardStorageLocationDTO { get; set; }

        // Virtuelles DbSet für "DashboardDwellTimeDTO"
        public virtual DbSet<DashboardDwellTimeDTO> DashboardDwellTimeDTO { get; set; }

        // Tabelle "PcbTypes"
        public DbSet<PcbType> PcbTypes
        {
            get; set;
        }

        // Tabelle "Users"
        public DbSet<User> Users
        {
            get; set;
        }

        // Tabelle "Transfers"
        public DbSet<Transfer> Transfers
        {
            get; set;
        }

        // Virtuelles DbSet für "EvaluationStorageLocationDTO"
        public virtual DbSet<EvaluationStorageLocationDTO> EvaluationStorageLocationDTO { get; set; }

        // Virtuelles DbSet für "EvaluationFinalizedDTO"
        public virtual DbSet<EvaluationFinalizedDTO> EvaluationFinalizedDTO { get; set; }

        // Tabelle "AuditEntries"
        public DbSet<AuditEntry> AuditEntries { get; set; }

        // Die Methode OnConfiguring wird aufgerufen, wenn die DbContext-Instanz konfiguriert wird.
        // Hier wird die Verbindungszeichenfolge für die Datenbankkonfiguration aus der AppSettings-Datei geholt und verwendet.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationHelper.Configuration.GetConnectionString("BoschContext"));
        }

        // Die Methode SaveChanges wird aufgerufen, wenn Änderungen an der Datenbank gespeichert werden sollen.
        // Hier wird das CreatedDate-Attribut für neue BaseEntity-Objekte gesetzt.
        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && e.State == EntityState.Added);
            foreach (var entry in entries)
            {
                if (((BaseEntity)entry.Entity).CreatedDate == DateTime.MinValue)
                {
                    ((BaseEntity)entry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        // Die asynchrone Version von SaveChanges.
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && e.State == EntityState.Added);
            foreach (var entry in entries)
            {
                if (((BaseEntity)entry.Entity).CreatedDate == DateTime.MinValue)
                {
                    ((BaseEntity)entry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
