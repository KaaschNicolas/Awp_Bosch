using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Backend.DataAccess
{
    public class BoschContext : DbContext
    {
        public BoschContext(DbContextOptions<BoschContext> options) : base(options) { }
        public DbSet<Anmerkung> Anmerkungen { get; set; }
        public DbSet<Diagnose> Diagnosen { get; set; }
        public DbSet<Fehlertyp> Fehlertypen { get; set; }
        public DbSet<Geraet> Geraete { get; set; }
        public DbSet<LagerOrt> LagerOrte { get; set; }
        public DbSet<Leiterplatte> Leiterplatten { get; set; }
        public DbSet<Leiterplattentyp> Leiterplattentypen { get; set; }
        public DbSet<Leiterplatte> Leiterplatten { get; set; }
        public DbSet<Nutzer> Nutzende { get; set; }
        public DbSet<Umbuchung> Umbuchungen { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=TestDB;User ID=sa;Password=Nicolas!1234;TrustServerCertificate=True");
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is BasisEntitaet && e.State == EntityState.Added);
            foreach (var entry in entries)
            {
                ((BasisEntitaet)entry.Entity).CreatedDate = DateTime.Now;
            }

            return base.SaveChanges();
        }
    }
}
