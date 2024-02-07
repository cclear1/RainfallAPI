# RainfallAPI

The API will start up and be accessible at `https://localhost:5001` by default.

## Usage

### Get Rainfall Readings by Station ID

GET /rainfall/id/{stationId}/readings

#### Parameters

- `stationId` (required): The ID of the weather station for which rainfall readings are requested.
- `count` (optional): The number of readings to retrieve. Default is 10.

#### Responses

- `200 OK`: Successfully retrieved rainfall readings. Returns a `RainfallReadingResponse` object.
- `400 Bad Request`: Invalid request parameters.
- `404 Not Found`: The specified weather station ID was not found.
- `500 Internal Server Error`: An unexpected error occurred.

## Built With

- .NET 5 - The framework used
- ASP.NET Core - Web framework
- Entity Framework Core - Object-relational mapping (ORM) framework
- Swagger - API documentation and testing tool

## Authors

- Callum Clear
