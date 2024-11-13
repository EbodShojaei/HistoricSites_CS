# Exotic Historic Sites

C# .NET Blazor App with SQLite database to view and manage exotic historic sites.

## Setup

```bash
dotnet clean

# Remove existing database
rm -f Data/Database/ExoticHistoricSites.db

# Remove existing migrations
rm -rf Migrations

# Build the solution
dotnet build

# Create new migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update

# Run the application
dotnet run
```
