using Profiles.Application.DTOs;
using Profiles.Domain.Models;

namespace Profiles.Application.Abstractions;

public interface IDoctorRepository : IRepository<DoctorProfile>
{
    Task<bool> EmailExistsAsync(string email);
    Task<PagedResult<DoctorProfile>> GetFilteredDoctorsAsync(DoctorFilterDto filter, bool onlyAtWork);
}