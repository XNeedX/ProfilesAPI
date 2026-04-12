using InnoClinic.Profiles.Api.DTOs;
using InnoClinic.Profiles.Api.Models;

namespace InnoClinic.Profiles.Api.Abstractions;

public interface IDoctorService
{
    Task<DoctorProfile> CreateDoctorAsync(DoctorProfileRegistrationRequest request);
    Task<IEnumerable<DoctorCardResponse>> GetFilteredDoctorsAsync(DoctorFilterRequest filter);
    Task<DoctorViewByDoctorResponse?> GetDoctorProfileByIdAsync(Guid id);
    Task UpdateDoctorAsync(Guid id, DoctorUpdateRequest request);
    Task<IEnumerable<DoctorTableRowResponse>> GetDoctorsForReceptionistAsync(DoctorFilterRequest filter);
}
