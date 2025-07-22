# Request and Response Logging using HttpLogging in .NET Core

This project demonstrates how to implement HTTP request and response logging in a .NET Core Web API application using the built-in `HttpLogging` middleware.

## Features

- Comprehensive HTTP request and response logging
- Configurable logging fields (headers, query parameters, request/response bodies)
- Environment-specific logging configuration
- Size limits for request and response body logging
- API endpoints for demonstration
- API Key authentication using custom middleware
- Custom HTTP logging interceptors for sensitive data handling
- HTTP client logging with DelegatingHandler

## Project Structure

- WebApi - ASP.NET Core Web API project
  - Endpoints - Contains API endpoint definitions
  - DTO - Data Transfer Objects
  - Middleware - Custom middleware components
  - Interceptors - HTTP logging interceptors
  - Clients - HTTP client implementations
  - Handlers - HTTP message handlers
  - Models - Data models

## Configuration

The project configures HTTP logging in `Program.cs` with the following options:

- Logs all HTTP fields by default
- Selectively logs specific request and response headers
- Limits request/response body sizes to prevent excessive logging
- Enhanced logging in development environment

### API Key Authentication

The application uses a custom middleware to enforce API Key authentication:

- All requests must include an `X-API-Key` header
- The API key is validated against the configuration value
- Swagger and OpenAPI endpoints are excluded from authentication
- Invalid or missing API keys result in 401/403 responses

### Custom Logging Interceptors

The application implements multiple logging interceptors:

1. **CustomLoggingInterceptor**:
   - Removes sensitive headers (like API keys) from logs
   - Adds custom parameters to request/response logs
   - Prevents logging of sensitive cookies

2. **SensitiveDataReductionInterceptor**:
   - Disables logging for POST requests
   - Redacts request paths for privacy
   - Redacts request and response headers
   - Enriches logs with additional custom fields

### HTTP Client Logging

The application demonstrates HTTP client logging:

- `HttpLoggingHandler` - A DelegatingHandler that logs outgoing HTTP requests and responses
- `TodoClient` - A typed HTTP client that consumes an external API
- Automatic API key inclusion in outgoing requests
- Proper error handling and response processing

## API Endpoints

- `/weatherforecast` - GET endpoint that returns weather forecast data
- `/api/users/login` - POST endpoint for user login (accepts LoginModel in request body)
- `/api/users/logout` - GET endpoint for user logout
- `/api/todos` - CRUD endpoints for todo items (proxied to external API)

## Usage

1. Clone the repository
2. Open the solution in Visual Studio or your preferred IDE
3. Run the application
4. Use the Swagger UI or HTTP client to test the endpoints
5. Include the `X-API-Key` header with the value from appsettings.json in all requests
6. Check the console/logs to see the HTTP request and response details

## Technologies

- .NET 9.0
- ASP.NET Core
- HttpLogging middleware
- HttpClient factory
- OpenAPI/Swagger