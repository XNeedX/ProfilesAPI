using Profiles.Application.DTOs;
using Profiles.Domain.Models;

namespace Profiles.Application.Abstractions;

public interface IPatientRepository : IRepository<PatientProfile>
{
    Task<PatientProfile?> GetByAccountIdAsync(string accountId);
}