using App.Core.DTOs;
using App.Core.Helpers;
using App.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

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

        // Virtuelles DbSet für "EvaluationPcbTypeI_ODTO"
        public virtual DbSet<EvaluationPcbTypeI_ODTO> EvaluationPcbTypeI_ODTO { get; set; }

        // Tabelle "AuditEntries"
        public DbSet<AuditEntry> AuditEntries { get; set; }

        // jedes Dict ist eine Reihe, Key = Spaltenname & Value = Wert an Stelle
        public List<Dictionary<string, object>> GenerateDTO(string sqlQuery)
        {
            List<Dictionary<string, object>> dto = new List<Dictionary<string, object>>();

            if (sqlQuery != null)
            {
                using (var connection = (SqlConnection)this.Database.GetDbConnection())
                {
                    connection.Open();
                    //this.Database.OpenConnection();
                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        //command.CommandText = sqlQuery;

                        using (var reader = command.ExecuteReader())
                        {
                            // Spaltennamen und -typen abrufen
                            DataTable schemaTable = reader.GetSchemaTable();
                            List<string> columnNames = new List<string>();
                            foreach (DataRow row in schemaTable.Rows)
                            {
                                string columnName = row["ColumnName"].ToString();
                                columnNames.Add(columnName);
                            }

                            // Daten in das DTO übertragen
                            while (reader.Read())
                            {
                                Dictionary<string, object> data = new Dictionary<string, object>();
                                foreach (string columnName in columnNames)
                                {
                                    object value = reader[columnName];
                                    data[columnName] = value;
                                }
                                dto.Add(data);
                            }
                        }
                    }
                }
            }

            return dto;
        }

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
