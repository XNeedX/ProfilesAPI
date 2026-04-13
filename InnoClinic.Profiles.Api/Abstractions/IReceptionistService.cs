using InnoClinic.Profiles.Api.DTOs;
using InnoClinic.Profiles.Api.Models;

namespace InnoClinic.Profiles.Api.Abstractions;

public interface IReceptionistService
{
    Task<Receptionist> CreateReceptionistAsync(CreateReceptionistRequest request);
    Task<ReceptionistViewRequest> GetReceptionistAsync(Guid id);
    Task UpdateReceptionistAsync(Guid id, ReceptionistUpdateRequest request);
    Task<IEnumerable<ReceptionistViewRequest>> GetAllAsync();
    Task DeleteReceptionistAsync(Guid id);
}