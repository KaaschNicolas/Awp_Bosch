using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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

        public DbSet<AuditEntry> AuditEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=127.0.0.1;Initial Catalog=TestDB;User ID=sa;Password=Nicolas!1234;TrustServerCertificate=True");
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && e.State == EntityState.Added);
            foreach (var entry in entries)
            {
                ((BaseEntity)entry.Entity).CreatedDate = DateTime.Now;
            }

            return base.SaveChanges();
        }
    }
}
