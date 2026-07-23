using Profiles.Application.DTOs;
using Profiles.Application.Abstractions;
using Profiles.Domain.Models;

namespace Profiles.Application.Abstractions;

public interface IReceptionistService
{
    Task<Result<Receptionist>> CreateReceptionistAsync(CreateReceptionistRequest request);
    Task<Result<ReceptionistProfileDto>> GetReceptionistAsync(Guid id);
    Task<Result> UpdateReceptionistAsync(Guid id, UpdateReceptionistDto request);
    Task<Result<PagedResult<ReceptionistProfileDto>>> GetAllPagedAsync(PageParams pageParams);
    Task<Result> DeleteReceptionistAsync(Guid id);
}