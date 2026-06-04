using InnoClinic.Contracts.Events.Profiles;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Profiles.Application.Abstractions;
using Profiles.Application.Abstractions;
using Profiles.Application.DTOs;
using Profiles.Application.Extensions;
using Profiles.Application.Mappings; 
using Profiles.Domain.Models;

namespace Profiles.Application.Services;

internal class ReceptionistService : IReceptionistService
{
    private readonly IRepository<Receptionist> _receptionistRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IEmailService _emailService;
    private readonly IPublishEndpoint _publishEndpoint;

    public ReceptionistService(
        IRepository<Receptionist> receptionistRepository,
        IUnitOfWork unitOfWork,
        IPasswordGenerator passwordGenerator,
        IEmailService emailService,
        IPublishEndpoint publishEndpoint)
    {
        _receptionistRepository = receptionistRepository;
        _unitOfWork = unitOfWork;
        _passwordGenerator = passwordGenerator;
        _emailService = emailService;
        _publishEndpoint = publishEndpoint;
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

        await _publishEndpoint.Publish<IReceptionistCreatedEvent>(new
        {
            Id = receptionist.Id,
            Email = receptionist.Email,
            FirstName = receptionist.FirstName,
            LastName = receptionist.LastName,
            MiddleName = receptionist.MiddleName,
            OfficeAddress = receptionist.OfficeAddress
        });

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

        await _publishEndpoint.Publish<IReceptionistUpdatedEvent>(new
        {
            Id = profile.Id,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            MiddleName = profile.MiddleName,
            OfficeAddress = profile.OfficeAddress
        });

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

        await _publishEndpoint.Publish<IReceptionistDeletedEvent>(new
        {
            Id = id
        });

        return Result.Success();
    }
}