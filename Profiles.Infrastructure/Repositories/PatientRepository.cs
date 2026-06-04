using Microsoft.EntityFrameworkCore;
using Profiles.Application.Abstractions;
using Profiles.Domain.Models;

namespace Profiles.Infrastructure.Repositories;
internal class PatientRepository : Repository<PatientProfile>, IPatientRepository
{
    public PatientRepository(ProfilesDbContext context) : base(context) { }

    public async Task<PatientProfile?> GetByAccountIdAsync(string accountId)
    {
        return await _dbSet.FirstOrDefaultAsync(p =>
            p.AccountId == accountId && p.IsLinkedToAccount);
    }
}