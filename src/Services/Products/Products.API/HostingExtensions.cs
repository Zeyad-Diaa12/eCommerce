using BuildingBlocks.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Products.API;

public static class HostingExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(Program).Assembly;
        var connectionString = configuration.GetConnectionString("Products");

        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

        services.AddMediatR(
            config =>
            {
                config.RegisterServicesFromAssembly(assembly);
                config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
                config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            }
        );
        
        services.AddValidatorsFromAssembly(assembly);

        services.AddCarter();

        services.AddMarten(opts =>
        {
            opts.Connection(connectionString);
        }).UseLightweightSessions();

        if (services.BuildServiceProvider().GetService<IHostEnvironment>().IsDevelopment())
        {
            services.InitializeMartenWith<InitialSeedData>();
        }

        services.AddExceptionHandler<CustomExcpetionHandler>();

        services.AddHealthChecks()
            .AddNpgSql(connectionString);

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products.API", Version = "v1" });
            var securitySchema = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "JWT Authorization header using the Bearer scheme",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            };

            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securitySchema);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }
            };
            c.AddSecurityRequirement(securityRequirement);
        });

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });

        services.AddAuthentication(
            options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireSuperAdminRole",
                policy => policy.RequireRole("SuperAdmin"));

            options.AddPolicy("RequireAdminRole",
                policy => policy.RequireRole("Admin", "SuperAdmin"));

            options.AddPolicy("GeneralAccess",
                policy => policy.RequireRole("User", "Admin", "SuperAdmin"));
        });

        return services;
    }
}
