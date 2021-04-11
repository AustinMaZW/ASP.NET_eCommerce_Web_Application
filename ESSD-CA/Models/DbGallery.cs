using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSD_CA.Models
{
    public class DbGallery : DbContext
    {
        protected IConfiguration configuration;

        public DbGallery(DbContextOptions<DbGallery> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<User>().HasIndex(x => x.Username).IsUnique();
        }
    }
}
