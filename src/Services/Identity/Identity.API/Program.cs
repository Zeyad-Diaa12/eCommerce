using BuildingBlocks.Exceptions.Handler;
using Carter;
using HealthChecks.UI.Client;
using Identity.Application;
using Identity.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


var assembly = typeof(Program).Assembly;
var connectionString = builder.Configuration.GetConnectionString("Identity");

builder.Services.AddCarter();

builder.Services.AddExceptionHandler<CustomExcpetionHandler>();

builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString!);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity.API", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddIdentityApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();


app.MapCarter();

app.UseExceptionHandler(options => { });

//CORS
app.UseCors();

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    }
);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
