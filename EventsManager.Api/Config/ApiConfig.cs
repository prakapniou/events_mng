using EventsManager.Api.Middleware.ExceptionHandling;
using EventsManager.Application.DTOs;
using EventsManager.Application.Interfaces;
using EventsManager.Application.Mapping;
using EventsManager.Application.Services;
using EventsManager.Application.Validation;
using FluentValidation;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using IdentityServer4.AccessTokenValidation;
using Serilog;
using ILogger = Serilog.ILogger;
using Microsoft.EntityFrameworkCore;
using EventsManager.Infrastructure.Persistance.EF.Contexts;
using EventsManager.Domain.Aggregates.SpeakerAggregate;
using EventsManager.Infrastructure.Persistance.Repositories;
using EventsManager.Domain.Aggregates.SponsorAggregate;
using EventsManager.Domain.Aggregates.EventAggregate;
using EventsManager.Infrastructure.MessageBrokers;

namespace EventsManager.Api.Config;

public class ApiConfig
{
    public static void SetConfiguration(
        IConfiguration configuration,
        IServiceCollection services,
        ILoggingBuilder logging)
    {
        SetDbContext(configuration, services);
        SetServices(services);
        SetAuth(services);
    }

    public static ILogger SetLogger(IConfiguration configuration, ILoggingBuilder logging)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        logging.ClearProviders();
        logging.AddSerilog(logger);

        return logger;
    }

    public static void SetMiddleware(WebApplication application)
    {
        if (application.Environment.IsDevelopment())
        {
            application.UseSwagger();
            application.UseSwaggerUI();
        }

        application.UseHttpsRedirection();
        application.UseAuthorization();
        application.UseAuthentication();
        application.MapControllers();
        application.UseMiddleware<ExceptionHandler>();
    }

    private static void SetServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddTransient<ExceptionHandler>();
        services.AddScoped<ISpeakerRepository,SpeakerRepository>();
        services.AddScoped<ISponsorRepository, SponsorRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddAutoMapper(typeof(MapProfile));
        services.AddScoped<IValidator<EventDTO>, EventValidator>();
        services.AddScoped<IValidator<SpeakerDTO>, SpeakerValidator>();
        services.AddScoped<IValidator<SponsorDTO>, SponsorValidator>();
        services.AddScoped<ISpeakerService, SpeakerService>();
        services.AddScoped<ISponsorService, SponsorService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IMessageBroker, RabbitMqMessageBroker>();

        services.AddDataProtection().UseCryptographicAlgorithms(
            new AuthenticatedEncryptorConfiguration
            {
                EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
            });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Meetup managment",
                Description = "Management Api on the example of events based on .Net 7.0",
                TermsOfService = new Uri("https://example.com/terms"),

                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Url = new Uri("https://example.com/contact")
                },

                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = new Uri("https://example.com/license")
                }
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            options.EnableAnnotations();
        });
    }

    private static void SetAuth(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
            });

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

                    Array.Empty<string>()
                }
            });
        });

        services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme) //bearer is default schema
            .AddIdentityServerAuthentication(options =>
            {
                //the URL on which the IdentityServer is up and running
                options.Authority="http://localhost:44390";
                //the name of the WebAPI resource
                options.ApiName="myApi";
                //select false for the development
                options.RequireHttpsMetadata= false;
            });
    }

    private static void SetDbContext(IConfiguration configuration, IServiceCollection services)
    {
        var connectionName = "DockerPosgreSqlDev";
        var connectionString = configuration
            .GetConnectionString(connectionName)
            ?? throw new InvalidOperationException($"Connection \"{connectionName}\" not found");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString, _ => _.EnableRetryOnFailure());
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });
    }
}
