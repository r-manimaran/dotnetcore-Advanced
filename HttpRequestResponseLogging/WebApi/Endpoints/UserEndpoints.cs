using Microsoft.AspNetCore.Mvc;
using WebApi.DTO;

namespace WebApi.Endpoints;

public static class UserEndpoints
{ 
    public static void MapUserEndpoints(this IEndpointRouteBuilder route)
    {
        var api = route.MapGroup("/api/users").WithTags("Users");

        api.MapPost("login",(LoginModel request, ILogger<Program> logger) =>
        {
            logger.LogInformation($"Processing the login for the user {request.Username}");
            return Results.Ok($"Welcome to the App {request.Username}");
        });

        api.MapGet("logout", (ILogger<Program> logger) =>
        {
            logger.LogInformation("Processing the logout");
            return Results.Ok("You have been logged out successfully.");
        });
    }
}
