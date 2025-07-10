# Cookie Authentication Minimal API

A .NET 9 Minimal API demonstrating secure cookie-based authentication with comprehensive security configurations.

## Features

- **Cookie Authentication**: Secure session-based authentication
- **Role-Based Authorization**: Policy-driven access control with hierarchical roles
- **Authorization Policies**: Custom policies for different access levels
- **Claims-Based Security**: User roles stored as claims
- **Security**: HttpOnly, Secure, SameSite cookie policies
- **Swagger/OpenAPI**: Interactive API documentation with security metadata
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

| Method | Endpoint | Description | Auth Required | Role Required |
|--------|----------|-------------|---------------|---------------|
| POST | `/login` | User authentication | No | - |
| GET | `/secure` | Protected resource | Yes | Any authenticated user |
| POST | `/logout` | User logout | No | - |
| GET | `/managearea` | Area management (Policy) | Yes | AreaManager |
| GET | `/managestore` | Store management (Policy) | Yes | StoreManager, AreaManager |
| GET | `/managearea2` | Area management (Direct) | Yes | AreaManager |
| GET | `/managestore2` | Store management (Attribute) | Yes | StoreManager, AreaManager |

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

## Authentication & Authorization Flow

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
**Response**: Sets `AppCookie` with user claims including `StoreManager` role

### Access Protected Resource
```http
GET /secure
Cookie: AppCookie=<session-cookie>
```

### Role-Based Endpoints
```http
# Store Management (StoreManager or AreaManager)
GET /managestore
Cookie: AppCookie=<session-cookie>

# Area Management (AreaManager only)
GET /managearea
Cookie: AppCookie=<session-cookie>
```

### Logout
```http
POST /logout
Cookie: AppCookie=<session-cookie>
```

## Security Configuration

### Cookie Settings
- **Cookie Name**: `AppCookie`
- **Expiration**: 10 minutes with sliding expiration
- **Security**: HTTPS only, HttpOnly, SameSite Strict
- **Unauthorized Response**: 401 status (no redirects)
- **Access Denied Response**: 403 status (no redirects)

### Authorization Policies
- **AreaManagement**: Requires `AreaManager` role
- **StoreManagement**: Requires `StoreManager` or `AreaManager` role
- **Role Hierarchy**: AreaManager > StoreManager

### Implementation Approaches
1. **Policy-based**: `.RequireAuthorization("PolicyName")`
2. **Direct role check**: `.RequireAuthorization(policy => policy.RequireRole("Role"))`
3. **Attribute-based**: `[Authorize(Roles = "Role1, Role2")]`

## Test Credentials

- **Username**: `test`
- **Password**: `password`
- **Assigned Role**: `StoreManager`

### Role Access Matrix
| Endpoint | StoreManager | AreaManager |
|----------|--------------|-------------|
| `/secure` | ✅ | ✅ |
| `/managestore` | ✅ | ✅ |
| `/managearea` | ❌ | ✅ |

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