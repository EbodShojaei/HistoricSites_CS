using ExoticHistoricSites.Server.Data;
using ExoticHistoricSites.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ExoticHistoricSites.Server.Repositories;

public class HistoricSiteRepository : IHistoricSiteRepository
{
    private readonly ApplicationDbContext _context;

    public HistoricSiteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<HistoricSite>> GetAllAsync()
    {
        return await _context.HistoricSites.ToListAsync();
    }

    public async Task<HistoricSite?> GetByIdAsync(int id)
    {
        return await _context.HistoricSites.FindAsync(id);
    }

    public async Task<IEnumerable<SiteImage>> GetSiteImagesAsync(int siteId)
    {
        return await _context
            .HistoricSiteImages.Where(hi => hi.HistoricSiteId == siteId)
            .Join(_context.SiteImages, hi => hi.SiteImageId, si => si.Id, (hi, si) => si)
            .ToListAsync();
    }

    public async Task<HistoricSite> CreateAsync(HistoricSite site)
    {
        _context.HistoricSites.Add(site);
        await _context.SaveChangesAsync();
        return site;
    }

    public async Task<HistoricSite?> UpdateAsync(int id, HistoricSite site)
    {
        var existingSite = await GetByIdAsync(id);
        if (existingSite == null)
            return null;

        existingSite.Name = site.Name;
        existingSite.Description = site.Description;
        existingSite.Countries = site.Countries;
        existingSite.Latitude = site.Latitude;
        existingSite.Longitude = site.Longitude;
        existingSite.MainImageBase64 = site.MainImageBase64;

        await _context.SaveChangesAsync();
        return existingSite;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var site = await _context.HistoricSites.FindAsync(id);
        if (site == null)
            return false;

        _context.HistoricSites.Remove(site);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddRatingAsync(int siteId, int userId, int rating)
    {
        var site = await GetByIdAsync(siteId);
        if (site == null)
            return false;

        var existingRating = await _context.SiteRatings.FirstOrDefaultAsync(r =>
            r.HistoricSiteId == siteId && r.UserId == userId
        );

        if (existingRating != null)
        {
            existingRating.Rating = rating;
        }
        else
        {
            _context.SiteRatings.Add(
                new SiteRating
                {
                    HistoricSiteId = siteId,
                    UserId = userId,
                    Rating = rating,
                }
            );
        }

        await _context.SaveChangesAsync();

        // Update average rating
        var averageRating = await _context
            .SiteRatings.Where(r => r.HistoricSiteId == siteId)
            .AverageAsync(r => (decimal)r.Rating);

        site.AverageRating = averageRating;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AddImageAsync(int siteId, string imageBase64)
    {
        var site = await GetByIdAsync(siteId);
        if (site == null)
            return false;

        var image = new SiteImage { ImageBase64 = imageBase64 };
        _context.SiteImages.Add(image);
        await _context.SaveChangesAsync();

        var junction = new HistoricSiteImage { HistoricSiteId = siteId, SiteImageId = image.Id };
        _context.HistoricSiteImages.Add(junction);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveImageAsync(int siteId, int imageId)
    {
        var junction = await _context.HistoricSiteImages.FirstOrDefaultAsync(hi =>
            hi.HistoricSiteId == siteId && hi.SiteImageId == imageId
        );

        if (junction == null)
            return false;

        _context.HistoricSiteImages.Remove(junction);
        await _context.SaveChangesAsync();

        // Check if image is used by other sites
        var isImageUsed = await _context.HistoricSiteImages.AnyAsync(hi =>
            hi.SiteImageId == imageId
        );

        if (!isImageUsed)
        {
            var image = await _context.SiteImages.FindAsync(imageId);
            if (image != null)
            {
                _context.SiteImages.Remove(image);
                await _context.SaveChangesAsync();
            }
        }

        return true;
    }
}
