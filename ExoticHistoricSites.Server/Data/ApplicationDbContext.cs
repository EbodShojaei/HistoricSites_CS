using ExoticHistoricSites.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ExoticHistoricSites.Server.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<HistoricSite> HistoricSites => Set<HistoricSite>();
    public DbSet<SiteImage> SiteImages => Set<SiteImage>();
    public DbSet<HistoricSiteImage> HistoricSiteImages => Set<HistoricSiteImage>();
    public DbSet<UserFavorite> UserFavorites => Set<UserFavorite>();
    public DbSet<SiteRating> SiteRatings => Set<SiteRating>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure HistoricSiteImages junction table
        modelBuilder
            .Entity<HistoricSiteImage>()
            .HasKey(hi => new { hi.HistoricSiteId, hi.SiteImageId });

        // Configure UserFavorites junction table
        modelBuilder.Entity<UserFavorite>().HasKey(uf => new { uf.UserId, uf.HistoricSiteId });

        // Configure SiteRating
        modelBuilder.Entity<SiteRating>().HasKey(sr => sr.Id);

        modelBuilder
            .Entity<SiteRating>()
            .HasIndex(sr => new { sr.UserId, sr.HistoricSiteId })
            .IsUnique();
    }
}
