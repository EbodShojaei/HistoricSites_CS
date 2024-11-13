using System.Text.Json;
using ExoticHistoricSites.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ExoticHistoricSites.Server.Data;

public static class DataSeeder
{
    private static readonly JsonSerializerOptions JsonOptions =
        new() { PropertyNameCaseInsensitive = true };

    public static async Task SeedAsync(ApplicationDbContext context)
    {
        try
        {
            await SeedUsersAsync(context);
            await SeedHistoricSitesAsync(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding data: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }

    private static async Task SeedUsersAsync(ApplicationDbContext context)
    {
        if (context.Users.Any())
        {
            Console.WriteLine("Users already seeded.");
            return;
        }

        // Correct path handling using Path.Combine and AppContext.BaseDirectory
        var usersFilePath = Path.Combine(
            AppContext.BaseDirectory,
            "Data",
            "SeedData",
            "users.json"
        );
        Console.WriteLine($"Users file path: {usersFilePath}");

        if (!File.Exists(usersFilePath))
        {
            Console.WriteLine($"Users file not found at path: {usersFilePath}");
            return;
        }

        var json = await File.ReadAllTextAsync(usersFilePath);
        var data = JsonSerializer.Deserialize<UserSeedData>(json, JsonOptions);

        if (data?.Users == null)
            return;

        foreach (var user in data.Users)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
        Console.WriteLine("Users seeded successfully.");
    }

    private static async Task SeedHistoricSitesAsync(ApplicationDbContext context)
    {
        if (context.HistoricSites.Any())
        {
            Console.WriteLine("Historic sites already seeded.");
            return;
        }

        // Correct path handling using Path.Combine and AppContext.BaseDirectory
        var sitesFilePath = Path.Combine(
            AppContext.BaseDirectory,
            "Data",
            "SeedData",
            "historicsites.json"
        );
        Console.WriteLine($"Historic sites file path: {sitesFilePath}");

        if (!File.Exists(sitesFilePath))
        {
            Console.WriteLine($"Historic sites file not found at path: {sitesFilePath}");
            return;
        }

        var json = await File.ReadAllTextAsync(sitesFilePath);
        var data = JsonSerializer.Deserialize<HistoricSiteSeedData>(json, JsonOptions);

        if (data?.HistoricSites == null)
            return;

        foreach (var site in data.HistoricSites)
        {
            context.HistoricSites.Add(site);
        }

        await context.SaveChangesAsync();
        Console.WriteLine("Historic sites seeded successfully.");
    }
}

public class UserSeedData
{
    public List<User> Users { get; set; } = new();
}

public class HistoricSiteSeedData
{
    public List<HistoricSite> HistoricSites { get; set; } = new();
}
