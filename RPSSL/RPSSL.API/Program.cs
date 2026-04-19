using Microsoft.EntityFrameworkCore;
using RPSSL.API.Domain.Interfaces;
using RPSSL.API.Extensions;
using RPSSL.API.Infrastructure.External;
using RPSSL.API.Infrastructure.External.Options;
using RPSSL.API.Infrastructure.Persistence.Configuration;
using RPSSL.API.Infrastructure.Repositories;
using RPSSL.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<InMemoryDbContext>(options =>
    options.UseInMemoryDatabase("RpSslDb"));

builder.Services.Configure<CodeChallengeApiOptions>(
    builder.Configuration.GetSection(CodeChallengeApiOptions.SectionName));

builder.Services.AddHttpClient<IRandomNumberService, RandomNumberService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["CodeChallengeApi:BaseAddress"]!);
});

builder.Services.AddScoped<IChoiceService, ChoiceService>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();

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
