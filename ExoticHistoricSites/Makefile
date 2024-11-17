.PHONY: help setup cleanup run

help:
	@echo "Available commands:"
	@echo "  make help    - Display this help message"
	@echo "  make setup   - Setup the project (cleanup, migrations, seeding)"
	@echo "  make cleanup - Cleanup the project (delete bin/obj folders, database, migrations)"
	@echo "  make run     - Run the application"

clean:
	@echo "Cleaning up the project..."
	dotnet clean
	rm -rf ExoticHistoricSites.API/bin ExoticHistoricSites.API/obj
	rm -f ExoticHistoricSites.API/Data/Db/ExoticHistoricSites.db*
	rm -rf ExoticHistoricSites.API/Migrations
	@echo "Cleanup completed."

setup: cleanup
	@echo "Setting up the project..."
	cd ExoticHistoricSites.API && dotnet build
	cd ExoticHistoricSites.API && dotnet ef migrations add InitialCreate
	cd ExoticHistoricSites.API && dotnet ef database update
	@echo "Setup completed."

run:
	@echo "Running the application..."
	cd ExoticHistoricSites.API && dotnet run
