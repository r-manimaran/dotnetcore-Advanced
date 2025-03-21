using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

//Serilog for Cloudwwatch
//The settings are specified in appSettings. This can be done through code in startup
builder.Host.UseSerilog((_, loggerConfig) =>
{
    loggerConfig.WriteTo.Console().ReadFrom.Configuration(builder.Configuration);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
