using System.Reflection;

namespace HouseBroker.Extensions;

public static class MediatorExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, Assembly assembly)
    {
        services.AddSingleton<IMediator, Mediator>();

        services.AddMediatorWithPipeline(assembly);
            

        var handlerInterface = typeof(IRequestHandler<,>);

        var handlers = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface)
                .Select(i => new { ServiceType = i, ImplementationType = t }));

        foreach (var handler in handlers)
        {
            services.AddTransient(handler.ServiceType, handler.ImplementationType);
        }

        return services;
    }

    public static IServiceCollection AddMediatorWithPipeline(this IServiceCollection services, Assembly assemblyToScan)
    {
        // Register IMediator
        services.AddTransient<IMediator, Mediator>();

        // Register all IRequestHandler<,>
        var handlerTypes = assemblyToScan
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces(), (impl, iface) => new { impl, iface })
            .Where(x =>
                x.iface.IsGenericType &&
                x.iface.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
            .ToList();

        foreach (var x in handlerTypes)
        {
            services.AddTransient(x.iface, x.impl);
        }

        // Register all IMiddleware<,> as open generics
        var middlewareTypes = assemblyToScan
            .GetTypes()
            .Where(t =>
                !t.IsAbstract &&
                !t.IsInterface &&
                t.IsGenericType &&
                t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IMiddleware<,>)))
            .ToList();

        foreach (var type in middlewareTypes)
        {
            // Find IMiddleware<,> interface
            var middlewareInterface = type.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMiddleware<,>));

            // Register the open generic
            var openGenericImpl = type.GetGenericTypeDefinition();
            var openGenericIface = middlewareInterface.GetGenericTypeDefinition();

            services.AddTransient(openGenericIface, openGenericImpl);
        }

        return services;
    }
}