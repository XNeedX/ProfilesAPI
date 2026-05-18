namespace Profiles.Application.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}