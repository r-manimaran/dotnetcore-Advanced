# Request and Response Logging using HttpLogging in .NET Core

This project demonstrates how to implement HTTP request and response logging in a .NET Core Web API application using the built-in `HttpLogging` middleware.

## Features

- Comprehensive HTTP request and response logging
- Configurable logging fields (headers, query parameters, request/response bodies)
- Environment-specific logging configuration
- Size limits for request and response body logging
- API endpoints for demonstration

## Project Structure

- WebApi - ASP.NET Core Web API project
  - Endpoints - Contains API endpoint definitions
  - DTO - Data Transfer Objects

## Configuration

The project configures HTTP logging in `Program.cs` with the following options:

- Logs all HTTP fields by default
- Selectively logs specific request and response headers
- Limits request/response body sizes to prevent excessive logging
- Enhanced logging in development environment

## API Endpoints

- `/weatherforecast` - GET endpoint that returns weather forecast data
- `/api/users/login` - POST endpoint for user login
- `/api/users/logout` - GET endpoint for user logout

## Usage

1. Clone the repository
2. Open the solution in Visual Studio or your preferred IDE
3. Run the application
4. Use the Swagger UI or HTTP client to test the endpoints
5. Check the console/logs to see the HTTP request and response details

## Technologies

- .NET 9.0
- ASP.NET Core
- HttpLogging middleware
- OpenAPI/Swagger