using MinimalApiFilters.Models;
using Dapper;
using System.Data;
using IDbConnectionFactory = MinimalApiFilters.Data.IDbConnectionFactory;

namespace MinimalApiFilters.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IDbConnectionFactory _connection;
        private readonly ILogger<CustomerService> _logger;
        public CustomerService(IDbConnectionFactory connection, ILogger<CustomerService> logger)
        {
            _connection = connection;
            _logger = logger;
        }
        public async Task<bool> CreateAsync(Customer customer)
        {
            using var connection = await _connection.CreateConnectionAsync();

            var sql = "INSERT INTO Customers (Id, FirstName, LastName, Email,DateOfBirth, CreatedAt,UpdatedAt) VALUES (@Id, @FirstName, @LastName, @Email,@DateOfBirth,@CreatedAt,@UpdatedAt)";
            var result = await connection.ExecuteAsync(sql, customer);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var connection = await _connection.CreateConnectionAsync();

            var sql = "DELETE FROM Customers WHERE Id = @Id";
            
            var customer = await connection.ExecuteAsync(sql, new { Id = id });
            
            return customer > 0;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            using var connection = await _connection.CreateConnectionAsync();

            var sql="SELECT * FROM Customers";

            var customers = await connection.QueryAsync<Customer>(sql);

            return customers;            
        }

        public async Task<Customer> GetAsync(Guid id)
        {
            using var connection = await _connection.CreateConnectionAsync();

            var sql = "SELECT * FROM Customers WHERE Id = @Id";
            var customer = await connection.QueryFirstOrDefaultAsync<Customer>(sql, new { Id = id });
            if (customer == null)
            {
                _logger.LogInformation($"Customer with id {id} not found");
                throw new KeyNotFoundException($"Customer with id {id} not found");
            }
            return customer;
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            using var connection = await _connection.CreateConnectionAsync();

            var sql = "UPDATE Customers SET FirstName = @Name, LastName = @LastName, Email = @Email WHERE Id = @Id";
            var result = await connection.ExecuteAsync(sql, customer);
            return result > 0;
        }
    }
}
