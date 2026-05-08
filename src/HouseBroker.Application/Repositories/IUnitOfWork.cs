namespace HouseBroker.Application.Repositories.Abstracts;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}