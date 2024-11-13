using ExoticHistoricSites.Shared.Models;

namespace ExoticHistoricSites.Server.Repositories;

public interface IHistoricSiteRepository
{
    Task<IEnumerable<HistoricSite>> GetAllAsync();
    Task<HistoricSite?> GetByIdAsync(int id);
    Task<IEnumerable<SiteImage>> GetSiteImagesAsync(int siteId);
    Task<HistoricSite> CreateAsync(HistoricSite site);
    Task<HistoricSite?> UpdateAsync(int id, HistoricSite site);
    Task<bool> DeleteAsync(int id);
    Task<bool> AddRatingAsync(int siteId, int userId, int rating);
    Task<bool> AddImageAsync(int siteId, string imageBase64);
    Task<bool> RemoveImageAsync(int siteId, int imageId);
}
