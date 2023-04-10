﻿using Entityframework_MSSQL_Test.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entityframework_MSSQL_Test.DataAccess
{
    public class UserContext : DbContext
    {
        private readonly string _connectionString;
        public UserContext(string connectionString)
        {
            
            if (String.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("ConnectionString is empty");
            }

            _connectionString = connectionString;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Email> Emails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
