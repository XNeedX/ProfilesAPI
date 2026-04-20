using Profiles.Application.DTOs;
using Profiles.Domain.Abstractions;
using Profiles.Domain.Models;

namespace Profiles.Application.Abstractions;

public interface IPatientService
{
    //Task<PatientProfile?> GetByIdAsync(Guid id);
    //Task<IEnumerable<PatientProfile>> GetAllAsync();
    Task<Result> UpdateAsync(Guid id, CreatePatientDto request);
    Task<Result> DeleteAsync(Guid id);

    Task<Result> LinkExistingProfileAsync(Guid profileId, string accountId);
    Task<Result<PatientProfile?>> FindMatchAsync(CreatePatientDto request);
    Task<Result<PatientProfile>> ConfirmAndCreateAsync(CreatePatientDto request, string accountId);
    Task<Result<PatientProfile>> CreateInitialAsync(string email);
    Task<Result<PatientProfileDto>> GetByAccountIdAsync(string accountId);
    Task<Result<PatientProfileDto>> GetPatientByIdAsync(Guid id);
}

