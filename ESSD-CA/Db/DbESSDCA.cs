using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using ESSD_CA.Models;
using Microsoft.EntityFrameworkCore;

namespace ESSD_CA.Db
{
    public class DbESSDCA : DbContext
    {
        protected IConfiguration configuration;
        public DbESSDCA(DbContextOptions<DbESSDCA> options)
            : base(options)
        {
            // options like which database provider to use (e.g.
            // MS SQL, Oracle, SQL Lite, MySQL
        }
        protected override void OnModelCreating(ModelBuilder model)
        {
            // fluent API, add code as per necessary
            // e.g. unique columns in a table
            // composite keys
        }

        // create tables
        public DbSet<Product> Products { get; set; }
    }
}
