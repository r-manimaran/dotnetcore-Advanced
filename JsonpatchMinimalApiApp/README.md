# JSON Patch Minimal API Application

A .NET 9 minimal API application demonstrating JSON Patch operations for updating insurance policy data.

## Features

- **JSON Patch Support**: Implements RFC 6902 JSON Patch standard for partial resource updates
- **Minimal API**: Built using .NET 9 minimal API architecture
- **Insurance Policy Management**: Sample domain model for insurance policies
- **Multiple Patch Operations**: Supports replace, add, remove, move, copy, and test operations

## Project Structure

```
WebApi/
├── Models/
│   └── InsurancePolicy.cs      # Insurance policy domain model
├── Services/
│   ├── IInsuranceService.cs    # Service interface
│   └── InsuranceService.cs     # In-memory service implementation
├── Program.cs                  # Application entry point and API configuration
├── WebApi.csproj              # Project file with dependencies
└── WebApi.http                # HTTP request examples
```

## Dependencies

- **.NET 9.0**: Target framework
- **Microsoft.AspNetCore.JsonPatch**: JSON Patch implementation
- **Newtonsoft.Json**: JSON serialization (required for JsonPatch)

## API Endpoints

### PATCH /policies/update/{id}

Updates an insurance policy using JSON Patch operations.

**Parameters:**
- `id` (int): Policy ID to update

**Request Headers:**
- `Content-Type: application/json-patch+json`

**Response Codes:**
- `200 OK`: Policy updated successfully
- `400 Bad Request`: Invalid JSON Patch document
- `404 Not Found`: Policy not found

## JSON Patch Operations

### 1. Replace Operation
```json
[{
    "op": "replace",
    "path": "/customerName",
    "value": "Updated Customer Name"
}]
```

### 2. Add Operation
```json
[{
    "op": "add",
    "path": "/policyNumber",
    "value": "PN-12345"
}]
```

### 3. Remove Operation
```json
[{
    "op": "remove",
    "path": "/policyNumber"
}]
```

### 4. Move Operation
```json
[{
    "op": "move",
    "from": "/customerName",
    "path": "/policyHolder"
}]
```

### 5. Copy Operation
```json
[{
    "op": "copy",
    "from": "/customerName",
    "path": "/previousCustomerName"
}]
```

### 6. Test Operation
```json
[{
    "op": "test",
    "path": "/customerName",
    "value": "Expected Value"
}]
```

## Getting Started

### Prerequisites
- .NET 9.0 SDK
- Visual Studio 2022 or VS Code

### Running the Application

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd JsonpatchMinimalApiApp
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   cd WebApi
   dotnet run
   ```

4. **Access the API**
   - HTTPS: `https://localhost:7082`
   - HTTP: `http://localhost:5000` (if configured)

### Testing with HTTP Requests

Use the provided `WebApi.http` file in Visual Studio or VS Code with the REST Client extension to test the API endpoints.

## Sample Data

The application includes two sample insurance policies:

```json
[
  {
    "id": 1,
    "customerName": "Customer 1",
    "sumAssured": 5000.02
  },
  {
    "id": 2,
    "customerName": "Customer 2",
    "sumAssured": 6500.00
  }
]
```

## Key Learning Points

- **JSON Patch Standard**: Implementation of RFC 6902 for partial updates
- **Minimal APIs**: Modern .NET approach for lightweight APIs
- **Dependency Injection**: Service registration and consumption
- **HTTP Context Handling**: Manual request body reading for patch operations
- **Content Type Handling**: Proper handling of `application/json-patch+json`

## Notes

- This is a learning/demonstration project with in-memory data storage
- For production use, consider adding validation, error handling, and persistent storage
- The application uses Newtonsoft.Json for JSON Patch compatibility