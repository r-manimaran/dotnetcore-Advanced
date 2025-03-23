using EFCoreDbFirstApi.Data;
using Microsoft.EntityFrameworkCore;

namespace EFCoreDbFirstApi.Endpoints
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder route)
        {
            var group = route.MapGroup("/api/customers").WithTags("Customers").WithOpenApi();

            group.MapGet("", async (int pageSize, int pageNumber, AppDbContext context) =>
            {
                if (pageSize == 0) pageSize = 10;
                if (pageNumber == 0) pageNumber = 1;

                var results = await context.Customers
                                    .AsNoTracking()
                                    .Include(c=>c.CustomerTypes)
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();
                return Results.Ok(results);
            });
        }
    }
}
