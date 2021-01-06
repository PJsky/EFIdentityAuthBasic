using EFIdentityAuthBasic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EFIdentityAuthBasic.DatabaseContext
{
    public class DataContext : DbContext, IDbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext() : base() { }

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // connect to sql server database
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DbAuthTesting3;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public DbSet<User> Users { get; set; }


    }
}
