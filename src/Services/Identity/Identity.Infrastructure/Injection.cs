using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BuildingBlocks.Cache;
using Identity.Domain.Entites;
using Identity.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure;

public static class Injection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        var jwtSettings = config.GetSection("Jwt");
        var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config.GetConnectionString("Redis");
            options.InstanceName = "IdentityCache_";
        });

        services.AddScoped<ICacheService, CacheService>();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("Identity"))
        );

        services
            .AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ClockSkew = TimeSpan.Zero,
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var cache =
                            context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

                        var jwtToken = context.SecurityToken;
                        var jti = jwtToken.Id;

                        if (await cache.ExistsAsync($"blacklisted_tokens:{jti}"))
                        {
                            context.Fail("Token has been revoked");
                        }

                        var userId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                        if (string.IsNullOrEmpty(userId))
                        {
                            context.Fail("Invalid token claims");
                        }
                    },
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"Authentication failed: {context.Exception}");
                        return Task.CompletedTask;
                    },
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireSuperAdminRole", policy => policy.RequireRole("SuperAdmin"));

            options.AddPolicy(
                "RequireAdminRole",
                policy => policy.RequireRole("Admin", "SuperAdmin")
            );

            options.AddPolicy(
                "GeneralAccess",
                policy => policy.RequireRole("User", "Admin", "SuperAdmin")
            );
        });

        return services;
    }
}
