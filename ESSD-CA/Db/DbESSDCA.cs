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

            model.Entity<User>().HasIndex(x => x.Username).IsUnique();
        }

        // create tables
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderDetails> PODetails { get; set; }
    }
}
