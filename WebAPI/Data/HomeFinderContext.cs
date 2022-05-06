
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class HomeFinderContext : IdentityDbContext<ApplicationUser>
    {
        public HomeFinderContext(DbContextOptions<HomeFinderContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<SaleStatus> SaleStatuses { get; set; }
        public DbSet<Tenure> Tenures { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<Address> Adresses { get; set; }
        public DbSet<ExpressionOfInterest> ExpressionOfInterests { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Property> Properties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Skapar kompositnyckel av de två FK.
            modelBuilder.Entity<ExpressionOfInterest>()
                .HasKey(eoi => new { eoi.ApplicationUserId, eoi.PropertyId });

            // Konfigurera Foreign Keys i kopplingstabellen.
            modelBuilder.Entity<ExpressionOfInterest>()
                .HasOne(eoi => eoi.ApplicationUser)
                .WithMany(au => au.ExpressionOfInterests)
                .HasForeignKey(eoi => eoi.ApplicationUserId);
            modelBuilder.Entity<ExpressionOfInterest>()
                .HasOne(eoi => eoi.Property)
                .WithMany(p => p.ExpressionOfInterests)
                .HasForeignKey(eoi => eoi.PropertyId);

            // Gör att mäklaren inte kan raderas innan objektet har raderats.
            modelBuilder.Entity<Property>()
                .HasOne(p => p.EstateAgent)
                .WithMany(ea => ea.Properties)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
