using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Claims;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();

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
            }
        };
    });

builder.Services.AddAuthorization();

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
        new Claim(ClaimTypes.Name, request.UserName)
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

app.Run();
