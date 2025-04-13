using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Transactions;
using WebApi.Dtos;
using WebApi.Models;
using WebApi.Models.Orders;
using WebApi.Models.Products;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(connectionString,
    o=>o.MigrationsHistoryTable(HistoryRepository.DefaultTableName,"products")));

// For Read Replica scenario
//builder.Services.AddDbContext<OrdersDbContext>(options =>
//    options.UseSqlServer(connectionString)
//           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

builder.Services.AddDbContext<OrdersDbContext>(options =>
    options.UseSqlServer(connectionString, 
    o=>o.MigrationsHistoryTable(HistoryRepository.DefaultTableName,"orders")));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint(
        "/openapi/v1.json", "OpenAPI v1");
    });
}

app.MapGet("products", async (ProductDbContext productDbContext) =>
{
    return Results.Ok(await productDbContext.Products.Select(p => p.Id).ToArrayAsync());
    //var products = await productDbContext.Products.ToListAsync();
    //return products;
});

app.MapPost("orders", async (SubmitOrderRequest request, 
            ProductDbContext productDbContext,
            OrdersDbContext orderDbContext) =>
{
    var products = await productDbContext.Products
                    .Where(p => request.ProductIds.Contains(p.Id))
                    .AsNoTracking()
                    .ToListAsync();
    if(products.Count != request.ProductIds.Count)
    {
        return Results.BadRequest("Some products are missing");
    }
    //using var transaction = new SqlTransaction();
    //orderDbContext.Database.UseTransaction(transaction);
    //productDbContext.Database.UseTransaction(transaction);

    var order = new Order
    {
        Id = new Guid(),
        TotalPrice = products.Sum(p => p.Price),
        LineItems = products.Select(p => new LineItem
        {
            Id = Guid.NewGuid(),
            ProductId = p.Id,           
            Price = p.Price
        }).ToList()
    };
    orderDbContext.Orders.Add(order);

    await orderDbContext.SaveChangesAsync();

    return Results.Ok(order);
});


app.UseHttpsRedirection();

app.Run();

