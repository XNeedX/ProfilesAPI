 using Profiles.Application.Abstractions;

namespace Profiles.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProfilesDbContext _context;

    public UnitOfWork(ProfilesDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}