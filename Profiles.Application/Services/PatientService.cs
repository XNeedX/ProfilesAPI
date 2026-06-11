using InnoClinic.Contracts.Events.Profiles;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Profiles.Application.Abstractions;
using Profiles.Application.DTOs;
using Profiles.Application.Mappings; 
using Profiles.Domain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Profiles.Application.Services;

internal class PatientService : IPatientService
{
    private readonly IRepository<PatientProfile> _patientRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public PatientService(
        IRepository<PatientProfile> patientRepository, 
        IUnitOfWork unitOfWork, 
        IPublishEndpoint publishEndpoint)
    {
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Result> UpdateAsync(Guid id, CreatePatientDto request)
    {
        var existing = await _patientRepository.GetByIdAsync(id);

        if (existing == null)
            return PatientErrors.NotFound;

        request.UpdateEntity(existing);

        _patientRepository.Update(existing);
        await _unitOfWork.SaveChangesAsync();

        await _publishEndpoint.Publish<IPatientUpdatedEvent>(new
        {
            Id = existing.Id,
            FirstName = existing.FirstName,
            LastName = existing.LastName,
            MiddleName = existing.MiddleName,
            PhoneNumber = existing.PhoneNumber,
            DateOfBirth = existing.DateOfBirth
        });

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var profile = await _patientRepository.GetByIdAsync(id);

        if (profile == null)
            return PatientErrors.NotFound;

        _patientRepository.Delete(profile);
        await _unitOfWork.SaveChangesAsync();

        await _publishEndpoint.Publish<IPatientProfileDeletedEvent>(new
        {
            Id = id
        });

        return Result.Success();
    }
    public async Task<Result<PatientProfile>> CreateInitialAsync(string email)
    {
        var profile = new PatientProfile
        {
            Id = Guid.NewGuid(),
            IsEmailVerified = false,
            IsLinkedToAccount = false
        };

        await _patientRepository.AddAsync(profile);
        await _unitOfWork.SaveChangesAsync();

        return Result<PatientProfile>.Success(profile);
    }
    public async Task<Result<PatientProfile?>> FindMatchAsync(CreatePatientDto request)
    {
        var candidates = await _patientRepository.GetAllAsync();
        var bestMatch = candidates.FirstOrDefault(p => CalculateMatchScore(p, request) >= 13);

        if (bestMatch == null)
        {
            return Result<PatientProfile?>.Success(null);
        }

        if (bestMatch.IsLinkedToAccount)
            return PatientErrors.AlreadyLinked;

        return Result<PatientProfile?>.Success(bestMatch);
    }

    private int CalculateMatchScore(PatientProfile existing, CreatePatientDto incoming)
    {
        int score = 0;
        if (string.Equals(existing.FirstName, incoming.FirstName, StringComparison.OrdinalIgnoreCase)) score += 5;
        if (string.Equals(existing.LastName, incoming.LastName, StringComparison.OrdinalIgnoreCase)) score += 5;
        if (existing.MiddleName != null && string.Equals(existing.MiddleName, incoming.MiddleName, StringComparison.OrdinalIgnoreCase)) score += 5;
        if (existing.DateOfBirth.Date == incoming.DateOfBirth.Date) score += 3;
        return score;
    }

    public async Task<Result<PatientProfile>> ConfirmAndCreateAsync(CreatePatientDto request, string accountId)
    {
        var profile = request.ToEntity();
        profile.Id = Guid.NewGuid();
        profile.IsLinkedToAccount = true;
        profile.AccountId = accountId;

        await _patientRepository.AddAsync(profile);
        await _unitOfWork.SaveChangesAsync();

        await _publishEndpoint.Publish<IPatientCreatedEvent>(new
        {
            Id = profile.Id,
            AccountId = profile.AccountId,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            MiddleName = profile.MiddleName,
            PhoneNumber = profile.PhoneNumber,
            DateOfBirth = profile.DateOfBirth
        });

        return Result<PatientProfile>.Success(profile);
    }

    public async Task<Result> LinkExistingProfileAsync(Guid profileId, string accountId)
    {
        var profile = await _patientRepository.GetByIdAsync(profileId);

        if (profile == null)
        {
            return PatientErrors.NotFound;
        }

        profile.IsLinkedToAccount = true;
        profile.AccountId = accountId;

        _patientRepository.Update(profile);
        await _unitOfWork.SaveChangesAsync();

        await _publishEndpoint.Publish<IPatientProfileLinkedEvent>(new
        {
            Id = profile.Id,
            AccountId = accountId
        });

        return Result.Success();
    }

    public async Task<Result<PatientProfileDto>> GetByAccountIdAsync(string accountId)
    {
        var profile = await _patientRepository.Query()
            .FirstOrDefaultAsync(p => p.AccountId == accountId && p.IsLinkedToAccount);

        if (profile == null)
            return PatientErrors.NotFound;

        return Result<PatientProfileDto>.Success(profile.ToPatientViewResponse());
    }

    public async Task<Result<PatientProfileDto>> GetPatientByIdAsync(Guid id)
    {
        var profile = await _patientRepository.GetByIdAsync(id);

        if (profile == null)
            return PatientErrors.NotFound;

        return Result<PatientProfileDto>.Success(profile.ToPatientViewResponse());
    }
}