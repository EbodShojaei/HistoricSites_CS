# Exotic Historic Sites API

This project is a RESTful dotnet API for managing exotic historic sites. The API provides endpoints for managing historic sites, countries, and regions.

```bash
Author:     Ebod Shojaei
Date  :     2024-11-14
```

## Getting Started

To get started, clone the repository and run the API using Visual Studio or the dotnet CLI.

```bash
# run Makefile commands
make help
make clean
make build
make run
```

## API Endpoints

The API provides the following endpoints:

- GET /api/historicsites
- GET /api/historicsites/{id}
- POST /api/historicsites
- PUT /api/historicsites/{id}
- DELETE /api/historicsites/{id}

```bash
# Postman API Requests
Base URL: http://localhost:5000/api/HistoricSites

GET Requests (Can be tested directly in browser):
1. Get all sites:
   GET http://localhost:5000/api/HistoricSites

2. Get site by ID:
   GET http://localhost:5000/api/HistoricSites/1

3. Search by country:
   GET http://localhost:5000/api/HistoricSites/search?country=China

4. Search by name:
   GET http://localhost:5000/api/HistoricSites/search?name=Petra

5. Search by both:
   GET http://localhost:5000/api/HistoricSites/search?country=China&name=Mount

POST/PUT/DELETE Requests (Requires Postman):

6. Create new site:
   POST http://localhost:5000/api/HistoricSites
   Headers: Content-Type: application/json
   Body:
   {
       "name": "Test Site",
       "description": "Test Description",
       "countries": "Test Country",
       "latitude": 0,
       "longitude": 0,
       "imageBase64": "",
       "averageRating": 5,
       "totalReviews": 0
   }

7. Update site:
   PUT http://localhost:5000/api/HistoricSites/1
   Headers: Content-Type: application/json
   Body:
   {
       "id": 1,
       "name": "Updated Site",
       "description": "Updated Description",
       "countries": "Updated Country",
       "latitude": 0,
       "longitude": 0,
       "imageBase64": "",
       "averageRating": 5,
       "totalReviews": 0
   }

8. Delete site:
   DELETE http://localhost:5000/api/HistoricSites/1

9. Upload image:
   POST http://localhost:5000/api/HistoricSites/upload-image/1
   Headers: Content-Type: multipart/form-data
   Body: form-data
   Key: image
   Value: [select file]

   Try using the Swagger UI instead (http://localhost:5000/swagger)
```

## Attributes

The API provides the following attributes for historic sites:

- Name
- Description
- Countries
- Latitude
- Longitude
- ImageBase64
- AverageRating
- TotalReviews

## Acknowledgements

AI was used to develop and review this project. The API was developed using dotnet core and C#. The API was tested using Postman.
