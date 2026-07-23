using Profiles.Domain.Models;

namespace Profiles.Application.Abstractions;

public interface IReceptionistRepository : IRepository<Receptionist>
{
    Task<bool> EmailExistsAsync(string email);
    Task<PagedResult<Receptionist>> GetPagedAsync(PageParams pageParams);
}