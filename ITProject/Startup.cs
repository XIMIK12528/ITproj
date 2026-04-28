using DataAccsess.Context;
using ITProject.Extensions;
using ITProject.Middlewears;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _hostEnvironment;

    public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        _configuration = configuration;
        _hostEnvironment = hostEnvironment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(item: new JsonStringEnumConverter(
                    JsonNamingPolicy.CamelCase,
                    false));
            });

        services.AddDbContext<Context>(
            opt => opt.UseNpgsql(connectionString: _configuration.GetConnectionString("DefaultConnection"),
            x => x.MigrationsAssembly("Context")),
            ServiceLifetime.Transient,
            ServiceLifetime.Transient);

        services.AddCors(o => o.AddPolicy("policy",
            builder =>
            {
                builder.SetIsOriginAllowed(_ => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            }));

        services.AddJwtAuthentication(_configuration);
        services.AddAuthorization(options =>
        {
            options.AddPolicy("OfferIsAccepted",
                policy => policy.Requirements.Add(item: new OfferIsAcceptedRequirement()));
        });

        services.AddSwaggerGen();

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        //services.AddDbRepositories(_configuration);
        //services.AddRepositories();

        //services.AddSingleton<ErrorHandlerMiddleware>();
        //services.AddSingleton<IAuthorizationHandler, OfferIsAcceptedMiddleware>();
        //services.AddScoped<IAccountsPresentationService, AccountsPresentationService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        //app.UseMiddleware<ErrorHandlerMiddleware>();

        app.UseSerilogRequestLogging();

        app.UseSwagger();

        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Qedr.Platypus.WebApi v1"));

        app.UseRouting();

        app.UseCors("policy");

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}