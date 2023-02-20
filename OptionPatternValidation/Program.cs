using FluentValidation;
using Microsoft.Extensions.Options;
using OptionPatternValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

builder.Services.AddOptions<ExampleOptions>()
    .Bind(config.GetSection(ExampleOptions.SectionName))
    //.Validate(opt =>
    //{
    //    if (opt.RetryCount is <= 0 or > 9)
    //        return false;

    //    return true;
    //})
    .ValidateDataAnnotations()
    .ValidateOptionFluently()
    .ValidateOnStart();
    

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

app.MapGet("hello",(IOptions<ExampleOptions> opt)=> opt.Value);

app.Run();
