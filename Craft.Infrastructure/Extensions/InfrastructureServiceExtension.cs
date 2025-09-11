using Craft.Infrastructure.ExternalServices;
using Craft.Infrastructure.Interfaces;
using Craft.Infrastructure.Presistence;
using Craft.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserQueryRepository, UserQueryRepository>();
            services.AddScoped<IUserCommandRepository, UserCommandRepository>();
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            services.AddScoped<IProjectQueryRepository, ProjectQueryRepository>();
            services.AddScoped<IProjectCommandRepository, ProjectCommandRepository>();
            services.AddScoped<MailService>();

            return services;
        }
    }
}
