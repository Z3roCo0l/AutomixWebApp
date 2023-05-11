using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AutomixMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Pomelo.EntityFrameworkCore.MySql;
using System.Linq;


namespace AutomixMVC.Data
{
    public class AutomixDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AutomixDbContext(DbContextOptions<AutomixDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Food> FoodItems { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                if (connectionString is null)
                {
                    throw new InvalidOperationException("The connection string was not found.");
                }
                optionsBuilder.UseMySQL(connectionString);
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Food>()
                .Property(f => f.FoodTypeId)
                .HasConversion<int>();

            modelBuilder.Entity<Food>()
                .Property(f => f.DailyMenuType)
                .HasConversion<int?>();

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });

            base.OnModelCreating(modelBuilder);
        }
    }

}
