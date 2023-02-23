using Microsoft.EntityFrameworkCore;
using TechTestBackend.Business;
using TechTestBackend.Business.Abstraction;
using TestTestBackend.Data;
using TestTestBackend.Data.Repositories;
using TestTestBackend.Data.Repositories.Abstraction;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextFactory<SongStorageContext>(options => options.UseInMemoryDatabase("SongStorage"));

builder.Services.AddScoped<ISpotifyService, SpotifyService>();
builder.Services.AddScoped<ISpotifyApiClient, SpotifyApiClient>();
builder.Services.AddScoped<ISongStorageRepository, SongStorageRepository>();


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
