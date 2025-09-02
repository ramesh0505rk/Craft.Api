using Craft.Application.Extensions;
using Craft.Infrastructure.Extensions;

namespace Craft.Api.Extensions
{
	public static class ApiServiceExtensions
	{
		public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSwaggerExtension();
			services.AddCorsExtension(configuration);
			services.AddApplicationServices();
			services.AddInfrastructureServices();
			services.AddAuthentication(configuration);
			return services;
		}
	}
}
