using AcidentesMadrid.Infrastructure.Interfaces;
using AcidentesMadrid.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace AcidentesMadrid.Infrastructure;

/// <summary>
/// Configura y registra las dependencias de la aplicación.
/// </summary>
public static class DependenciesProvider {

    public static IServiceProvider BuildServiceProvider() {
        
        var services = new ServiceCollection();

        services.Scan(scan => scan
            .FromAssemblyOf<AccidenteRepository>()

            // Transient
            .AddClasses(classes => classes.AssignableTo<ITransientService>())
            .AsImplementedInterfaces()
            .WithTransientLifetime()

            // Scoped
            .AddClasses(classes => classes.AssignableTo<IScopedService>())
            .AsImplementedInterfaces()
            .WithScopedLifetime()

            // Singleton
            .AddClasses(classes => classes.AssignableTo<ISingletonService>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime()
        );

        return services.BuildServiceProvider();
    }
}