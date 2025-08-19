using Craft.Application.Mapper;
using Craft.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Application.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Register MediatR from application layer
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            //Register AutoMapper from application layer
            services.AddAutoMapper(typeof(MapperProfile).Assembly);

            //Register FluentValidation from application layer
            services.AddValidatorsFromAssemblyContaining<UserRegistrationRequestValidator>();

            services.AddFluentValidationAutoValidation(options =>
            {
                options.DisableDataAnnotationsValidation = true; // Disable Data Annotations validation
            });

            return services;
        }
    }
}
