using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System.Text.Json;
using WebApi.Models;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IInsuranceService, InsuranceService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPatch("/policies/update/{id:int}", async (int id, IInsuranceService insuranceService, HttpContext context) =>
{

    using StreamReader reader = new(context.Request.Body);
    var bodyString = await reader.ReadToEndAsync();

    var patchDoc = JsonConvert.DeserializeObject<JsonPatchDocument>(bodyString);
    if (patchDoc is null)
    {
        return Results.BadRequest("Invalid Json Patch document");
    }

    var policy = insuranceService.GetPolicies().FirstOrDefault(x => x.Id == id);
    if (policy is null)
    {
        return Results.NotFound("Policy not found!.");
    }
    patchDoc.ApplyTo(policy);

    return Results.Ok(policy);
}).Accepts<JsonPatchDocument<InsurancePolicy>>("application/json-patch+json")
.Produces(200)
.Produces(404)
.Produces(400);

app.Run();

