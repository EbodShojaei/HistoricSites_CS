using ExoticHistoricSites.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;

// Get the connection string and expand environment variables
var connectionString = configuration.GetConnectionString("DefaultConnection");

// Manually expand the %HOME% environment variable if present
if (!string.IsNullOrEmpty(connectionString) && connectionString.Contains("%HOME%"))
{
    var homePath = Environment.GetEnvironmentVariable("HOME") ?? "";
    connectionString = connectionString.Replace("%HOME%", homePath);
}

// Add DbContext with the expanded connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(connectionString);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ensure the database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var environment = services.GetRequiredService<IHostEnvironment>();

    // Ensure the database exists
    context.Database.EnsureCreated();

    if (configuration.GetValue<bool>("RunDatabaseSeeding"))
    {
        // Pass the environment to the seeder
        await DataSeeder.SeedAsync(context, environment);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
