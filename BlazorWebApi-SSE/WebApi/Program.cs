using SharedLib;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("blazorlocal",policy =>
    {
        policy.WithOrigins("https://localhost:7160")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<Random>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("blazorlocal");

static async IAsyncEnumerable<Humidity> StreamHumidity(Random random, [EnumeratorCancellation] CancellationToken cancellationToken)
{
    var rand = new Random();
    while (!cancellationToken.IsCancellationRequested)
    {
        yield return new Humidity { Percentage = rand.Next(1, 100) };

        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
    }
}

app.MapGet("/humidity/stream", (Random random, CancellationToken cancellationToken) =>
{
    return TypedResults.ServerSentEvents(StreamHumidity(random, cancellationToken),eventType:"humidity");
});

app.Run();


