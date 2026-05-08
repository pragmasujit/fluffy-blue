public interface IRequest<TResponse> { }

public interface IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

public interface IMiddleware<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken);
}

public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var responseType = typeof(TResponse);

        // 1. Resolve handler
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
        var handler = _serviceProvider.GetService(handlerType);
        if (handler == null)
            throw new InvalidOperationException($"No handler found for {requestType.Name}");

        // 2. Resolve middlewares
        var middlewareType = typeof(IMiddleware<,>).MakeGenericType(requestType, responseType);
        var middlewares = (_serviceProvider.GetService(typeof(IEnumerable<>).MakeGenericType(middlewareType)) as IEnumerable<object>)
            ?.Reverse()
            .ToList() ?? new List<object>();

        // 3. Define the core handler as the "next" delegate
        Func<CancellationToken, Task<TResponse>> handlerDelegate = async (ct) =>
        {
            var method = handlerType.GetMethod("Handle");
            return await (Task<TResponse>)method.Invoke(handler, new object[] { request, ct });
        };

        // 4. Wrap handler with middleware chain
        foreach (var middleware in middlewares)
        {
            var current = handlerDelegate; // capture current next
            handlerDelegate = (ct) =>
            {
                var method = middleware.GetType().GetMethod("Handle");
                return (Task<TResponse>)method.Invoke(middleware, new object[] { request, current, ct });
            };
        }

        return await handlerDelegate(cancellationToken);
    }
}
