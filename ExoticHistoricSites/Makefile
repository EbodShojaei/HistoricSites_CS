.PHONY: help setup clean run host

RESOURCE_GROUP = "ExoticSitesRG"
WEB_APP_NAME = "ExoticHistoricSitesApp"

help:
	@echo "Available commands:"
	@echo "  make help    - Display this help message"
	@echo "  make setup   - Setup the project (cleanup, migrations, seeding)"
	@echo "  make clean   - Cleanup the project (delete bin/obj folders, database, migrations)"
	@echo "  make run     - Run the application"
	@echo "  make host    - Deploy the application to Azure"

clean:
	@echo "Cleaning up the project..."
	dotnet clean
	rm -rf ExoticHistoricSites.API/bin ExoticHistoricSites.API/obj
	rm -f ExoticHistoricSites.API/Data/Db/ExoticHistoricSites.db*
	rm -rf ExoticHistoricSites.API/Migrations
	@echo "Cleanup completed."

setup: clean
	@echo "Setting up the project..."
	cd ExoticHistoricSites.API && dotnet build
	cd ExoticHistoricSites.API && dotnet ef migrations add InitialCreate
	cd ExoticHistoricSites.API && dotnet ef database update
	@echo "Seeding the database..."
	cd ExoticHistoricSites.API && dotnet run --seed
	@echo "Setup and seeding completed."

run:
	@echo "Running the application..."
	cd ExoticHistoricSites.API && dotnet run

host:
	@echo "Deploying to Azure..."
	dotnet publish -c Release -o ./publish
	zip -r publish.zip ./publish
	az webapp deployment source config-zip --resource-group $(RESOURCE_GROUP) --name $(WEB_APP_NAME) --src ./publish.zip
	@echo "Deployment completed."
	rm -f publish.zip
