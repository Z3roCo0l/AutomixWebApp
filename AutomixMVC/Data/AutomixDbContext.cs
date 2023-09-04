using Microsoft.EntityFrameworkCore;
using AutomixMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Pomelo.EntityFrameworkCore.MySql;
using System.Linq;
using System.Drawing.Text;

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
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Food> FoodItems { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<FoodIngredients> FoodIngredients { get; set; }
        public DbSet<FoodIngredientAssociation> FoodIngredientAssociations { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Setting> Settings { get; set; }

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

            // Seed the Roles
            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = 1,
                Name = "Admin"
            }, new Role
            {
                Id = 2,
                Name = "Kitchen"
            }, new Role
            {
                Id = 3,
                Name = "Waiter"
            });

            // Seed the initial admin user
            modelBuilder.Entity<User>().HasData(new User("admin", HashPassword("adminpassword"), "Admin")
            {
                Id = 1
            });

            modelBuilder.Entity<FoodIngredientAssociation>()
           .HasKey(fia => new { fia.FoodId, fia.FoodIngredientsID });

            modelBuilder.Entity<FoodIngredientAssociation>()
                .HasOne(fia => fia.Food)
                .WithMany(f => f.FoodIngredientAssociations)
                .HasForeignKey(fia => fia.FoodId);

            modelBuilder.Entity<FoodIngredientAssociation>()
                .HasOne(fia => fia.FoodIngredients)
                .WithMany(fi => fi.FoodIngredientAssociations)
                .HasForeignKey(fia => fia.FoodIngredientsID);

            modelBuilder.Entity<Setting>().HasData(
                new Setting { Id = 1, Name = "DefaultMenuPrice", Value = "1490.0" }  
            );
        }

        private string HashPassword(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(passwordHash);
            }
        }
    }
}