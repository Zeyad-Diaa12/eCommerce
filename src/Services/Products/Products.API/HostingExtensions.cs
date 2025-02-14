using Microsoft.OpenApi.Models;

namespace Products.API;

public static class HostingExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(Program).Assembly;
        var connectionString = configuration.GetConnectionString("Products");

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

        services.AddAuthentication();

        services.AddAuthorization();

        return services;
    }
}
