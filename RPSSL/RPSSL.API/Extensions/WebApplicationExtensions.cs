using RPSSL.API.Infrastructure.Persistence.Configuration;

namespace RPSSL.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void Configure(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "RPSSL API");
                    options.RoutePrefix = "swagger";
                });
            }

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseRouting();

            app.MapControllers();

            SeedDatabase(app);
        }

        private static void SeedDatabase(IHost app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<InMemoryDbContext>();
            context.Seed();
        }
    }
}
