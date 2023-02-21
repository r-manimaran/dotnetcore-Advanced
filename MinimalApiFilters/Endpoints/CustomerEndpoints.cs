using FluentValidation;
using MinimalApiFilters.Models;
using MinimalApiFilters.Services;
using System.Runtime.CompilerServices;

namespace MinimalApiFilters.Endpoints
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this WebApplication app)
        {
            app.MapGet("customers", GetAllCustomers);
            app.MapGet("customers/{id:guid}", GetCustomerById);
            app.MapPost("customers", CreateCustomer);
            app.MapDelete("customers/{id:guid}", DeleteCustomer);
        }

        public static async Task<IResult> DeleteCustomer(Guid Id,
                                    ICustomerService customerService)
        {
            var deleted = await customerService.DeleteAsync(Id);
            if(!deleted)
            {

            }
        }

        public static async Task GetCustomerById(Guid Id, 
                                    ICustomerService customerService) 
        {
            var customer = await customerService.GetAsync(id);
        }

        public static async Task<IResult> GetAllCustomers(ICustomerService customerService)
        {
            var customers = await customerService.GetAllAsync();
            return Results.Ok(customers);
        }

        public static async Task<IResult> CreateCustomer(Customer customer,
            IValidator<Customer> validator, ICustomerService customerService)
        {
            var result = await validator.ValidateAsync(customer);
            if (!result.IsValid)
            {
                return Results.BadRequest(result.Errors.ToResponse());
            }

            await customerService.CreateAsync(customer);
            return Results.Created($"/customers/{customer.Id}", customer);
        }


    }
}
