using Profiles.Application.DTOs;
using Profiles.Domain.Abstractions;
using Profiles.Domain.Models;

namespace Profiles.Application.Abstractions;

public interface IDoctorService
{
    Task<Result<DoctorProfile>> CreateDoctorAsync(CreateDoctorDto request);
    Task<Result<PagedResult<DoctorCardDto>>> GetFilteredDoctorsAsync(DoctorFilterDto filter);
    Task<Result<DoctorDetailsDto>> GetDoctorProfileByIdAsync(Guid id);
    Task<Result> UpdateDoctorAsync(Guid id, UpdateDoctorDto request);
    Task<Result<PagedResult<DoctorTableRowDto>>> GetDoctorsForReceptionistAsync(DoctorFilterDto filter);
}
