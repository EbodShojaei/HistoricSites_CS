using ExoticHistoricSites.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ExoticHistoricSites.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public required DbSet<HistoricSite> HistoricSites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HistoricSite>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd(); // Ensure ID is auto-generated

                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Countries).IsRequired();
                entity.Property(e => e.ImageBase64).IsRequired(false);
            });
        }
    }
}
