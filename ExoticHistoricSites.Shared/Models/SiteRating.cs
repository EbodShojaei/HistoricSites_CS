using System.ComponentModel.DataAnnotations;

namespace ExoticHistoricSites.Shared.Models;

public class SiteRating
{
    public int Id { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    public int UserId { get; set; }
    public int HistoricSiteId { get; set; }
}
