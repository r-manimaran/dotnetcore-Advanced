using FluentValidation;
using MinimalApiFilters.Filters;
using MinimalApiFilters.Models;
using MinimalApiFilters.Services;
using System.Runtime.CompilerServices;

namespace MinimalApiFilters.Endpoints
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/customers")
                           .WithTags("Customers")
                           .WithOpenApi();

            group.MapGet("customers", GetAllCustomers);

            group.MapGet("customers/{id:guid}", GetCustomerById);
            
            // Add EndPoint Filter to validate
            group.MapPost("customers", CreateCustomer)
                .AddEndpointFilter<ValidationFilter<Customer>>();
            
            group.MapDelete("customers/{id:guid}", DeleteCustomer);
        }

        public static async Task<IResult> DeleteCustomer(Guid Id,
                                    ICustomerService customerService)
        {
            var deleted = await customerService.DeleteAsync(Id);
            if(!deleted)
            {
                return Results.NotFound($"customer with ID {Id} not found.");
            }
            return Results.NoContent();
        }

        public static async Task<IResult> GetCustomerById(Guid Id, 
                                    ICustomerService customerService) 
        {
            var customer = await customerService.GetAsync(Id);
            if (customer is null)
            {
                return Results.NotFound($"customer with ID {Id} not found.");
            }
            return Results.Ok(customer);
        }

        public static async Task<IResult> GetAllCustomers(ICustomerService customerService)
        {
            var customers = await customerService.GetAllAsync();
            return Results.Ok(customers);
        }

        public static async Task<IResult> CreateCustomer(Customer customer,
            IValidator<Customer> validator, ICustomerService customerService)
        {
      
            await customerService.CreateAsync(customer);

            return Results.Created($"/customers/{customer.Id}", customer);
        }


    }
}
