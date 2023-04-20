using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AutomixMVC.Models;

namespace AutomixMVC.Data
{
    public class AutomixDbContext : IdentityDbContext
    {
        public AutomixDbContext(DbContextOptions<AutomixDbContext> options) : base(options) { 

        }

        public DbSet<Food> FoodItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Food>()
                .Property(f => f.FoodTypeId)
                .HasConversion<int>();

            modelBuilder.Entity<Food>()
                .Property(f => f.DailyMenuType)
                .HasConversion<int?>();
        }
    }
}
