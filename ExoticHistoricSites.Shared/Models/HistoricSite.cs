using System.ComponentModel.DataAnnotations;

namespace ExoticHistoricSites.Shared.Models;

public class HistoricSite
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }

    [Required]
    [StringLength(200)]
    public string Countries { get; set; } = string.Empty;

    public string? MainImageBase64 { get; set; }

    [Range(0, 5)]
    public decimal AverageRating { get; set; }
}
