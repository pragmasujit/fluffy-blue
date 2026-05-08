using Api.Shared.Exceptions;
using FluentValidation;

namespace Api.Shared.MediatorBehaviors;

public class ExceptionHandlingMiddleware<TRequest, TResponse> : IMiddleware<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (ValidationException ex)
        {
            throw new AppException(
                message: ex.Message
            );
        }
    }
}
