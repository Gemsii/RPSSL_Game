using Microsoft.EntityFrameworkCore;
using RPSSL.API.Extensions;
using RPSSL.API.Infrastructure.Persistence.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<InMemoryDbContext>(options =>
    options.UseInMemoryDatabase("RpSslDb"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

app.Configure();

app.Run();
