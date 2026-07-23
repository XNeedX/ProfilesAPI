 using Profiles.Application.Abstractions;

namespace Profiles.Infrastructure.Repositories;

internal  class UnitOfWork : IUnitOfWork, IDisposable
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

    public void Dispose()
    {
        _context.Dispose();
    }
}