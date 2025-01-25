using Dapper;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace MinimalApiFilters.Data
{
    public class DatabaseInitializer
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DatabaseInitializer(IDbConnectionFactory connectionFactory) {
            _connectionFactory = connectionFactory;
        }
        public async Task IntializeAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync(); ;
            
            // Create Customers table
            await connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Customers (
                    Id TEXT PRIMARY KEY,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE,
                    DateOfBirth TEXT NOT NULL,
                    CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
                    UpdatedAt TEXT NOT NULL DEFAULT (datetime('now'))
                )");

            // Create index for faster email lookups
            await connection.ExecuteAsync(@"
                CREATE INDEX IF NOT EXISTS idx_customers_email 
                ON Customers(Email)");

            // Optional: Add some seed data
            await SeedInitialData(connection);
        }

        private async Task SeedInitialData(IDbConnection connection)
        {
            // Check if we already have data
            var exists = await connection.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(*) FROM Customers");

            if (exists == 0)
            {
                var seedData = new[]
                {
                    new
                    {
                        Id = Guid.NewGuid().ToString(),
                        FirstName = "John",
                        LastName = "Doe",
                        Email = "john.doe@example.com",
                        DateOfBirth = DateTime.Parse("1990-01-01").ToString("yyyy-MM-dd"),
                        CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                        UpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                    },
                    new
                    {
                        Id = Guid.NewGuid().ToString(),
                        FirstName = "Jane",
                        LastName = "Smith",
                        Email = "jane.smith@example.com",
                        DateOfBirth = DateTime.Parse("1992-05-15").ToString("yyyy-MM-dd"),
                        CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                        UpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                    }
                };

                await connection.ExecuteAsync(@"
                    INSERT INTO Customers (Id, FirstName, LastName, Email, DateOfBirth, CreatedAt, UpdatedAt)
                    VALUES (@Id, @FirstName, @LastName, @Email, @DateOfBirth, @CreatedAt, @UpdatedAt)",
                    seedData);
            }
        }
    }
}
