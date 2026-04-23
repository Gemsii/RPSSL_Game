using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RPSSL.API.Domain.Interfaces;
using RPSSL.API.Features.Choices.GetChoices;
using RPSSL.API.Features.Choices.GetRandomChoice;
using RPSSL.API.Features.Games.GetScoreboard;
using RPSSL.API.Features.Games.Play;
using RPSSL.API.Features.Games.ResetScoreboard;
using RPSSL.API.Features.Players.Create;
using RPSSL.API.Infrastructure.External;
using RPSSL.API.Infrastructure.External.Options;
using RPSSL.API.Infrastructure.Persistence.Configuration;
using RPSSL.API.Infrastructure.Repositories;
using RPSSL.API.Domain.Services;

namespace RPSSL.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>Registers DbContext and repository implementations.</summary>
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RpsslDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();

            return services;
        }

        /// <summary>Registers the external random number API client and its options.</summary>
        public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CodeChallengeApiOptions>(
                configuration.GetSection(CodeChallengeApiOptions.SectionName));

            services.AddHttpClient<IRandomNumberService, RandomNumberService>(client =>
            {
                client.BaseAddress = new Uri(configuration["CodeChallengeApi:BaseAddress"]!);
            });

            return services;
        }

        /// <summary>Registers application-level domain services.</summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IChoiceService, ChoiceService>();

            return services;
        }

        /// <summary>Registers all feature handlers and validators.</summary>
        public static IServiceCollection AddFeatures(this IServiceCollection services)
        {
            services.AddScoped<CreatePlayerHandler>();
            services.AddScoped<IValidator<CreatePlayerCommand>, CreatePlayerCommandValidator>();

            services.AddScoped<PlayGameHandler>();
            services.AddScoped<IValidator<PlayGameCommand>, PlayGameCommandValidator>();

            services.AddScoped<GetChoicesHandler>();
            services.AddScoped<GetRandomChoiceHandler>();

            services.AddScoped<GetScoreboardHandler>();
            services.AddScoped<ResetScoreboardHandler>();

            return services;
        }

        /// <summary>Registers the CORS policy that allows any origin, method and header.</summary>
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });

            return services;
        }
    }
}
