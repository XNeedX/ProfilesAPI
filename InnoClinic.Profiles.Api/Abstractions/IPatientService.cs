using InnoClinic.Profiles.Api.DTOs;
using InnoClinic.Profiles.Api.Models;

namespace InnoClinic.Profiles.Api.Abstractions
{
    public interface IPatientService
    {
        //Task<PatientProfile?> GetByIdAsync(Guid id);
        //Task<IEnumerable<PatientProfile>> GetAllAsync();
        Task UpdateAsync(Guid id, PatientProfileRegistrationRequest request);
        Task DeleteAsync(Guid id);

        Task LinkExistingProfileAsync(Guid profileId, string accountId);
        Task<PatientProfile?> FindMatchAsync(PatientProfileRegistrationRequest request);
        Task<PatientProfile> ConfirmAndCreateAsync(PatientProfileRegistrationRequest request, string accountId);
        Task<PatientProfile> CreateInitialAsync(string email);
        Task<PatientProfileViewByPatientResponse?> GetByAccountIdAsync(string accountId);
        Task<ProfileViewResponse?> GetPatientByIdAsync(Guid id);
    }
}
