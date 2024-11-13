using System.Net.Http.Json;
using ExoticHistoricSites.Shared.Models;

namespace ExoticHistoricSites.Client.Services;

public class HistoricSiteService
{
    private readonly HttpClient _http;

    public HistoricSiteService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<HistoricSite>> GetAllSitesAsync()
    {
        try
        {
            var response = await _http.GetFromJsonAsync<List<HistoricSite>>("api/historicsites");
            if (response != null)
            {
                Console.WriteLine($"Successfully fetched {response.Count} sites");
                return response;
            }
            return new List<HistoricSite>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching sites: {ex.Message}");
            return new List<HistoricSite>();
        }
    }
}
