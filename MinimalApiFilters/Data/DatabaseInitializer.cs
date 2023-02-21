using Dapper;


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
            await connection.ExecuteAsync(@"");
        }
    }
}
