using Microsoft.Extensions.Configuration;

namespace Craft.Api.Extensions
{
    public static class CorsExtension
    {
        public static IServiceCollection AddCorsExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        var origins = configuration.GetSection("CORS:AllowedOrigins").Get<string[]>();
                        builder.WithOrigins(origins: origins)
                                 .AllowAnyMethod()
                                 .AllowAnyHeader();
                    });
            });
            return services;
        }
    }
}
