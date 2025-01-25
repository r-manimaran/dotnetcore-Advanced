using System.Data;
using Microsoft.Data.Sqlite;

namespace MinimalApiFilters.Data;

public class SqliteConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqliteConnectionFactory(string nameOrConnectionString)
    {
        _connectionString = nameOrConnectionString;
    }
    
    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}
