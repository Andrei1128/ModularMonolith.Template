using Core.Common.AppSettings;
using Core.Contracts.Persistence;
using Core.Infrastructure;
using Core.Persistence;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

            config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);

        return services;
    }

    public static IServiceCollection BindAppSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var config = new ConfigurationBuilder()
            .Add(new DecryptConfigurationSource(configuration))
            .Build();

        services.Configure<ConnectionStrings>(config.GetSection(ConnectionStrings.Key));

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddPersistence();

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        var connStrings = services.BuildServiceProvider().GetService<IOptions<ConnectionStrings>>()?.Value ??
            throw new InvalidOperationException("IOptions<ConnectionStrings> not found!");

        services.AddDbContext<WeatherManagementDbContext>(options => options.UseSqlServer(connStrings.SqlServer));
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();


        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<WeatherManagementDbContext>());

        return services;
    }
}
