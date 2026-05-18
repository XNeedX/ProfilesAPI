using Microsoft.EntityFrameworkCore;
using Profiles.Application.Abstractions;
using Profiles.Application.DTOs;
using Profiles.Application.Extensions;
using Profiles.Application.Mappings; 
using Profiles.Domain.Abstractions;
using Profiles.Domain.Models;

namespace Profiles.Application.Services;

public class ReceptionistService : IReceptionistService
{
    private readonly IRepository<Receptionist> _receptionistRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IEmailService _emailService;

    public ReceptionistService(
        IRepository<Receptionist> receptionistRepository,
        IUnitOfWork unitOfWork,
        IPasswordGenerator passwordGenerator,
        IEmailService emailService)
    {
        _receptionistRepository = receptionistRepository;
        _unitOfWork = unitOfWork;
        _passwordGenerator = passwordGenerator;
        _emailService = emailService;
    }

    public async Task<Result<Receptionist>> CreateReceptionistAsync(CreateReceptionistRequest request)
    {
        var emailExists = await _receptionistRepository.Query().AnyAsync(r => r.Email == request.Email);
        if (emailExists)
        {
            return ReceptionistErrors.DuplicateEmail;
        }

        var generatedPassword = _passwordGenerator.GeneratePassword();

        var receptionist = request.ToEntity();
        receptionist.Id = Guid.NewGuid();

        await _receptionistRepository.AddAsync(receptionist);
        await _unitOfWork.SaveChangesAsync();

        await _emailService.SendCredentialsAsync(receptionist.Email, generatedPassword);

        return Result<Receptionist>.Success(receptionist);
    }

    public async Task<Result<ReceptionistProfileDto>> GetReceptionistAsync(Guid id)
    {
        var receptionist = await _receptionistRepository.GetByIdAsync(id);

        if (receptionist == null)
            return ReceptionistErrors.NotFound;

        return Result<ReceptionistProfileDto>.Success(receptionist.ToViewResponse());
    }

    public async Task<Result> UpdateReceptionistAsync(Guid id, UpdateReceptionistDto request)
    {
        var profile = await _receptionistRepository.GetByIdAsync(id);

        if (profile == null)
        {
            return ReceptionistErrors.NotFound;
        }

        request.UpdateEntity(profile);

        _receptionistRepository.Update(profile);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<PagedResult<ReceptionistProfileDto>>> GetAllPagedAsync(PageParams pageParams)
    {
        var query = _receptionistRepository.Query();
        var pagedEntities = await query.ToPagedAsync(pageParams);

        var dtos = pagedEntities.Data
            .Select(r => r.ToViewResponse())
            .ToArray();

        var pagedResult = new PagedResult<ReceptionistProfileDto>(dtos, pagedEntities.TotalCount);

        return Result<PagedResult<ReceptionistProfileDto>>.Success(pagedResult);
    }

    public async Task<Result> DeleteReceptionistAsync(Guid id)
    {
        var profile = await _receptionistRepository.GetByIdAsync(id);

        if (profile == null)
            return ReceptionistErrors.NotFound;

        _receptionistRepository.Delete(profile);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}