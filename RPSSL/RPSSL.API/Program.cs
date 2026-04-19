using RPSSL.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPersistence()
    .AddExternalServices(builder.Configuration)
    .AddApplicationServices()
    .AddFeatures()
    .AddCorsPolicy()
    .AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Configure();

app.Run();

