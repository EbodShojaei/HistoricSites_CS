using System.Text.Json;
using ExoticHistoricSites.API.Models;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ExoticHistoricSites.API.Data;

public static class DataSeeder
{
    private static readonly JsonSerializerOptions JsonOptions =
        new() { PropertyNameCaseInsensitive = true };

    public static async Task SeedAsync(ApplicationDbContext context, IHostEnvironment environment)
    {
        try
        {
            await SeedHistoricSitesAsync(context, environment);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding data: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }

    private static async Task SeedHistoricSitesAsync(
        ApplicationDbContext context,
        IHostEnvironment environment
    )
    {
        Console.WriteLine("Starting data seeding process...");
        Console.WriteLine(
            $"Current Environment: {(environment.IsDevelopment() ? "Development" : "Production")}"
        );

        if (await context.HistoricSites.AnyAsync())
        {
            Console.WriteLine("Historic sites already seeded.");
            return;
        }

        // Use AppContext.BaseDirectory
        string baseDir = AppContext.BaseDirectory;
        Console.WriteLine($"Base Directory: {baseDir}");

        var jsonPath = Path.Combine(baseDir, "Data", "SeedData", "historicSites.json");
        var imagesDir = Path.Combine(baseDir, "Data", "Images");

        Console.WriteLine($"Looking for seed file at: {jsonPath}");
        Console.WriteLine($"Looking for images in: {imagesDir}");

        if (!File.Exists(jsonPath))
        {
            Console.WriteLine($"Seed file not found at: {jsonPath}");
            return;
        }
        else
        {
            Console.WriteLine($"Found seed file at: {jsonPath}");
        }

        if (!Directory.Exists(imagesDir))
        {
            Console.WriteLine($"Images directory not found at: {imagesDir}");
            return;
        }
        else
        {
            Console.WriteLine($"Found images directory at: {imagesDir}");
        }

        // Read and deserialize the JSON file
        var json = await File.ReadAllTextAsync(jsonPath);
        var data = JsonSerializer.Deserialize<SeedDataWrapper>(json, JsonOptions);

        if (data?.HistoricSites == null)
        {
            Console.WriteLine("No historic sites data found in JSON file.");
            return;
        }

        // Process each historic site
        foreach (var site in data.HistoricSites)
        {
            var imagePath = Path.Combine(imagesDir, $"{site.Id}.jpg");

            if (File.Exists(imagePath))
            {
                try
                {
                    // Load and process the image
                    using var image = await Image.LoadAsync(imagePath);

                    // Calculate new dimensions maintaining aspect ratio
                    var aspectRatio = (float)image.Height / image.Width;
                    var newWidth = 300;
                    var newHeight = (int)(newWidth * aspectRatio);

                    // Resize the image
                    image.Mutate(x => x.Resize(newWidth, newHeight));

                    // Convert to base64
                    using var memStream = new MemoryStream();
                    await image.SaveAsJpegAsync(memStream);
                    var base64String = Convert.ToBase64String(memStream.ToArray());

                    // Set the base64 string with data URI scheme
                    site.ImageBase64 = $"data:image/jpeg;base64,{base64String}";

                    Console.WriteLine($"Processed image for site ID {site.Id}: {site.Name}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Error processing image for site ID {site.Id}: {ex.Message}"
                    );
                    // Set a default or placeholder image if needed
                    site.ImageBase64 = null;
                }
            }
            else
            {
                Console.WriteLine($"Image not found for site ID {site.Id}: {imagePath}");
                site.ImageBase64 = null;
            }

            await context.HistoricSites.AddAsync(site);
            Console.WriteLine($"Added site ID {site.Id} to context.");
        }

        await context.SaveChangesAsync();
        Console.WriteLine(
            $"Successfully seeded {data.HistoricSites.Count} historic sites with images."
        );
    }
}

public class SeedDataWrapper
{
    public List<HistoricSite> HistoricSites { get; set; } = new();
}
