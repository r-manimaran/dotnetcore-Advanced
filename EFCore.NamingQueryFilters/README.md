# EFCore Query Filter in .Net 9

Demonstration project showcasing EFCore Query Filters in .NET 9 using a Product management system with automatic filtering of inactive products.

## Features

- **Global Query Filters**: Automatically filters out inactive products using `HasQueryFilter`
- **ASP.NET Core Minimal APIs**: Clean endpoint definitions for product operations
- **Entity Framework Core 9.0**: Latest EF Core features and improvements
- **SQL Server Integration**: Uses SQL Server with Aspire for local development
- **Bogus Data Generation**: Fake data generation for testing

## Project Structure

```
EFCore.NamingQueryFilters/
├── Model/
│   └── Product.cs              # Product entity with IsActive property
├── Endpoints/
│   └── ProductsEndpoints.cs    # Minimal API endpoints
├── Extensions/
│   └── AppExtensions.cs        # Extension methods
├── Data/
│   └── DatabaseSeedService.cs  # Data seeding service
├── Migrations/                 # EF Core migrations
├── AppDbContext.cs            # DbContext with query filter
└── Program.cs                 # Application entry point
```

## Query Filter Implementation

The query filter is configured in `AppDbContext.OnModelCreating`:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Product>().HasQueryFilter(p => p.IsActive);
}
```

## Setup & Migration

### Create migrations
```bash
dotnet ef migrations add InitialCreate
```
![Initial Migration](image-1.png)
*Initial Migration*

### Run the application
```bash
dotnet run
```

## API Endpoints

- `GET /products` - Get all active products (filtered automatically)
- `GET /products/{id}` - Get specific product by ID (if active)

## .Net 9 EFCore Query Filter Generated SQL

```sql
SELECT [p].[Id], [p].[CreatedAt], [p].[IsActive], [p].[Name], [p].[Price]
FROM [Products] AS [p]
WHERE [p].[IsActive] = CAST(1 AS bit)
```

![Filtered Query Result](image.png)
*IsActive Filtered Output*

## Key Dependencies

- Microsoft.EntityFrameworkCore 9.0.8
- Microsoft.EntityFrameworkCore.SqlServer 9.0.8
- Aspire.Microsoft.Data.SqlClient 9.4.0
- Bogus 35.6.3