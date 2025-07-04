# Cookie Authentication Minimal API

A .NET 9 Minimal API demonstrating secure cookie-based authentication with comprehensive security configurations.

## Features

- **Cookie Authentication**: Secure session-based authentication
- **Authorization**: Protected endpoints with claims-based access
- **Security**: HttpOnly, Secure, SameSite cookie policies
- **Swagger/OpenAPI**: Interactive API documentation
- **Sliding Expiration**: Automatic session extension on activity

## Project Structure

```
WebApi/
├── Models/
│   └── LoginRequest.cs          # Login request model
├── Program.cs                   # Main application configuration
├── WebApi.csproj               # Project dependencies
├── WebApi.http                 # HTTP test requests
└── appsettings.json            # Application configuration
```

## API Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/login` | User authentication | No |
| GET | `/secure` | Protected resource | Yes |
| POST | `/logout` | User logout | No |

## Getting Started

### Prerequisites
- .NET 9 SDK
- Visual Studio 2022 or VS Code

### Running the Application

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd CookieAuthentication.MinimalApi/WebApi
   ```

2. **Run the application**
   ```bash
   dotnet run
   ```

3. **Access Swagger UI**
   ```
   https://localhost:5001/swagger
   ```

## Authentication Flow

### Login
```http
POST /login
Content-Type: application/json

{
  "userName": "test",
  "password": "password",
  "rememberMe": false
}
```

### Access Protected Resource
```http
GET /secure
Cookie: AppCookie=<session-cookie>
```

### Logout
```http
POST /logout
Cookie: AppCookie=<session-cookie>
```

## Security Configuration

- **Cookie Name**: `AppCookie`
- **Expiration**: 10 minutes with sliding expiration
- **Security**: HTTPS only, HttpOnly, SameSite Strict
- **Unauthorized Response**: 401 status (no redirects)

## Test Credentials

- **Username**: `test`
- **Password**: `password`

## Dependencies

- Microsoft.AspNetCore.OpenApi (9.0.6)
- Swashbuckle.AspNetCore (9.0.1)

## Enhancement Ideas

- Database integration with Entity Framework Core
- User registration and password hashing
- Role-based authorization
- Rate limiting and account lockout
- Two-factor authentication
- Keycloak/OAuth integration
- SignalR for real-time features

## Development Notes

- Built with .NET 9 Minimal APIs
- Uses cookie authentication instead of JWT for session management
- Configured for HTTPS in production
- Includes comprehensive security headers