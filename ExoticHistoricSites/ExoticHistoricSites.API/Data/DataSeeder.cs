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

    public static async Task SeedAsync(ApplicationDbContext context)
    {
        try
        {
            await SeedHistoricSitesAsync(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding data: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }

    private static async Task SeedHistoricSitesAsync(ApplicationDbContext context)
    {
        if (await context.HistoricSites.AnyAsync())
        {
            Console.WriteLine("Historic sites already seeded.");
            return;
        }

        // Build paths
        var currentDir = Directory.GetCurrentDirectory();
        var jsonPath = Path.Combine(currentDir, "Data", "SeedData", "historicSites.json");
        var imagesDir = Path.Combine(currentDir, "Data", "Images");

        Console.WriteLine($"Looking for seed file at: {jsonPath}");
        Console.WriteLine($"Looking for images in: {imagesDir}");

        if (!File.Exists(jsonPath))
        {
            Console.WriteLine($"Seed file not found at: {jsonPath}");
            return;
        }

        if (!Directory.Exists(imagesDir))
        {
            Console.WriteLine($"Images directory not found at: {imagesDir}");
            return;
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
                    site.ImageBase64 = "base64_encoded_image_here"; // You might want to provide a default image
                }
            }
            else
            {
                Console.WriteLine($"Image not found for site ID {site.Id}: {imagePath}");
                site.ImageBase64 = "base64_encoded_image_here"; // You might want to provide a default image
            }

            await context.HistoricSites.AddAsync(site);
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
