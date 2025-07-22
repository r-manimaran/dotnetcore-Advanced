using Microsoft.AspNetCore.Mvc;
using WebApi.Clients;
using WebApi.Models;

namespace WebApi.Endpoints
{
    public static class TodoEndpoints
    {
        public static void MapTodoEndpoints(this IEndpointRouteBuilder route)
        {
            var api = route.MapGroup("/api/todos").WithTags("Todos");

            api.MapGet("", async (ITodoClient todoClient) =>
            {
                return await todoClient.GetTodosAsync();
            });

            api.MapGet("{id}", async (int id, ITodoClient todoClient) =>
            {
                return await todoClient.GetTodoAsync(id);
            });

            api.MapPost("", async (TodoItem todoItem, ITodoClient todoClient) =>
            {
                return await todoClient.CreateTodoAsync(todoItem);
            });

            api.MapPut("{id}", async (int id, TodoItem todoItem, ITodoClient todoClient) =>
            {
                return await todoClient.UpdateTodoAsync(id, todoItem);
            });

            api.MapDelete("{id}", async (int id, ITodoClient todoClient) =>
            {
                await todoClient.DeleteTodoAsync(id);
                return Results.NoContent();
            });
        }
    }
}