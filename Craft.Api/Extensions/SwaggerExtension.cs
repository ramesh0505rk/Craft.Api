using Microsoft.OpenApi.Models;

namespace Craft.Api.Extensions
{
	public static class SwaggerExtension
	{
		public static IServiceCollection AddSwaggerExtension(this IServiceCollection services)
		{
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Craft API",
					Version = "v1",
					Description = "API for Craft application"
				});

				// Add JWT Bearer security definition
				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "Enter your valid JWT token."
				});

				// Add JWT Bearer security requirement
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						new string[] {}
					}
				});
			});
			return services;
		}

		public static IApplicationBuilder UseSwaggerExtension(this IApplicationBuilder app)
		{
			var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
			if (env.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.DisplayRequestDuration();
					c.DocumentTitle = "Craft API documentation";
				});
			}
			return app;
		}
	}
}
