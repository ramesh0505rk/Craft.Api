using Craft.Application.Extensions;

namespace Craft.Api.Extensions
{
    public static class ApiServiceExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerExtension();
            services.AddCorsExtension(configuration);
            services.AddApplicationServices();
            return services;
        }
    }
}
