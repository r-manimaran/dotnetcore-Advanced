using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Claims;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();

// with SupressChecForUnhandledSecurityMetadata = true, all the Api endpoints will remain "visible" in the OpenAPI documentation, even if the user fails the policy check.
// This is useful for development and testing purposes, but in production, you might want to set it to false to hide endpoints that the user cannot access.
builder.Services.Configure<RouteOptions>(ropt=> {
    ropt.SuppressCheckForUnhandledSecurityMetadata = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o=>
    {
        o.Cookie.Name = "AppCookie";
        o.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        o.SlidingExpiration = true;
        o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        o.Cookie.HttpOnly = true;
        o.Cookie.SameSite = SameSiteMode.Strict;
        o.Events = new CookieAuthenticationEvents()
        {
            OnRedirectToLogin = ctx =>
            {
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = ctx =>
            {
                ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }
        };
    });

// builder.Services.AddAuthorization();

// Below policy will have access to all endpoints. The logged in user must have a role of "AreaManager" to access the endpoints.
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AreaManagement", p=>p.RequireRole("AreaManager"))
    // Below policy will have access to specific endpoints.  The logged user must have a role of StoreManager or AreaManager.
    // In role hierarchy, AreaManager is higher than StoreManager.
    .AddPolicy("StoreManagement", p=>p.RequireRole("StoreManager", "AreaManager"));

var app = builder.Build();

app.MapOpenApi();

app.UseSwaggerUI(opt =>
    opt.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1"));

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapPost("/login", async (LoginRequestModel request, HttpContext ctx) =>
{
    if(request is null || request.UserName !="test" || request.Password != "password")
    {
        return Results.Unauthorized();
    }

    List<Claim> claims = [ 
        new Claim(ClaimTypes.Name, request.UserName),
        new Claim(ClaimTypes.Role, "StoreManager") // This roles needs to be replaced with actual user roles from your database or identity provider.
        ];

    ClaimsIdentity ci = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    ClaimsPrincipal cp = new ClaimsPrincipal(ci);

    await ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,cp, new AuthenticationProperties
    {
        IsPersistent = request.RememberMe,
        IssuedUtc = DateTime.UtcNow,
    });

    return Results.Ok(new { message = "Successfully Logged In" });


});

app.MapGet("/secure",(ClaimsPrincipal user)=>
{
    return Results.Ok(
        new 
        { 
            message = $"Logged In User:{user.Identity?.Name}" 
        });

}).RequireAuthorization();

app.MapPost("/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Ok(new { message = "Logged out" });
});


// Below endpoint is protected by the StoreManagement policy.
// Only users with StoreManager or AreaManager roles can access this endpoint.
app.MapGet("/managearea", () =>
{
    return Results.Ok(new { message = "Manage the whole area here" });
}).RequireAuthorization("AreaManagement");

// Below endpoint is protected by the AreaManagement policy.
// Only users with AreaManager role can access this endpoint.
app.MapGet("/managestore", () =>
{
    return Results.Ok(new { message = "Manage only the store here" });
}).RequireAuthorization("StoreManagement");

// Another Syntax to set the roles that can access the endpoint.
// here instead of using the policy, we are directly specifying the roles.
app.MapGet("/managearea2", () =>
{
    return Results.Ok(new { message = "Manage the whole area here v2" });
}).RequireAuthorization(policy => policy.RequireRole( "AreaManager"));

app.MapGet("/managestore2", [Authorize(Roles = "StoreManager, AreaManager")] () =>
{
    return Results.Ok(new { message = "Manage only the store here v2" });
});

app.Run();
