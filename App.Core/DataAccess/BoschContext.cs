using App.Core.DTOs;
using App.Core.Helpers;
using App.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App.Core.DataAccess
{
    public class BoschContext : DbContext
    {
        public BoschContext()
        {
        }
        public BoschContext(DbContextOptions<BoschContext> options) : base(options) { }
        public DbSet<Comment> Comments
        {
            get; set;
        }
        public DbSet<Diagnose> Diagnoses
        {
            get; set;
        }
        public DbSet<ErrorType> ErrorTypes
        {
            get; set;
        }
        public DbSet<Device> Devices
        {
            get; set;
        }
        public DbSet<StorageLocation> StorageLocations
        {
            get; set;
        }
        public DbSet<Pcb> Pcbs
        {
            get; set;
        }


        public virtual DbSet<PcbDTO> PcbsDTO { get; set; }

        public virtual DbSet<DwellTimeEvaluationDTO> DwellTimeEvaluationDTO { get; set; }


        public DbSet<PcbType> PcbTypes
        {
            get; set;
        }
        public DbSet<User> Users
        {
            get; set;
        }
        public DbSet<Transfer> Transfers
        {
            get; set;
        }

        public virtual DbSet<EvaluationStorageLocationDTO> EvaluationStorageLocationDTO { get; set; }

        public virtual DbSet<EvaluationFinalizedDTO> EvaluationFinalizedDTO { get; set; }

        public DbSet<AuditEntry> AuditEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationHelper.Configuration.GetConnectionString("BoschContext"));
        }

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
