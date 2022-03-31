using Microsoft.Net.Http.Headers;
using StarWars.Interfaces;
using StarWars.Services;
using StarWars.Swapi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<ISwapiService, StandardSwapiService>();
builder.Services.AddSingleton<IStarWarsCalculationService, StandardStarWarsCalculationService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("swapi", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["SwapiBaseUrl"]);
    httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
