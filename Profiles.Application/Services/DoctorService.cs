using InnoClinic.Contracts.Events.Profiles;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Profiles.Application.Abstractions;
using Profiles.Application.DTOs;
using Profiles.Application.Extensions;
using Profiles.Application.Mappings; 
using Profiles.Domain.Models;

namespace Profiles.Application.Services;

internal class DoctorService : IDoctorService
{
    private readonly IRepository<DoctorProfile> _doctorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IEmailService _emailService;
    private readonly IPublishEndpoint _publishEndpoint;

    public DoctorService(
        IRepository<DoctorProfile> doctorRepository,
        IUnitOfWork unitOfWork,
        IPasswordGenerator passwordGenerator,
        IEmailService emailService,
        IPublishEndpoint publishEndpoint)
    {
        _doctorRepository = doctorRepository;
        _unitOfWork = unitOfWork;
        _passwordGenerator = passwordGenerator;
        _emailService = emailService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Result<DoctorProfile>> CreateDoctorAsync(CreateDoctorDto request)
    {
        var emailExists = await _doctorRepository.Query().AnyAsync(d => d.Email == request.Email);
        if (emailExists)
            return DoctorErrors.DuplicateEmail;

        var generatedPassword = _passwordGenerator.GeneratePassword();

        var doctor = request.ToEntity();
        doctor.Id = Guid.NewGuid();

        await _doctorRepository.AddAsync(doctor);
        await _unitOfWork.SaveChangesAsync();

        await _emailService.SendCredentialsAsync(doctor.Email, generatedPassword);

        await _publishEndpoint.Publish<IDoctorCreatedEvent>(new
        {
            Id = doctor.Id,
            Email = doctor.Email,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            MiddleName = doctor.MiddleName,
            DateOfBirth = doctor.DateOfBirth,
            Specialization = doctor.Specialization,
            CareerStartYear = doctor.CareerStartYear,
            OfficeAddress = doctor.OfficeAddress
        });

        return Result<DoctorProfile>.Success(doctor);
    }

    public async Task<Result<DoctorDetailsDto>> GetDoctorProfileByIdAsync(Guid id)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);
        if (doctor == null)
            return DoctorErrors.NotFound;

        return Result<DoctorDetailsDto>.Success(doctor.ToDoctorViewResponse());
    }

    public async Task<Result<DoctorAdminDto>> GetDoctorProfileForAdminAsync(Guid id)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);
        if (doctor == null)
            return DoctorErrors.NotFound;

        return Result<DoctorAdminDto>.Success(doctor.ToAdminViewResponse());
    }

    public async Task<Result> UpdateDoctorAsync(Guid id, UpdateDoctorDto request)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id);

        if (doctor == null)
            return DoctorErrors.NotFound;

        request.UpdateEntity(doctor);

        _doctorRepository.Update(doctor);
        await _unitOfWork.SaveChangesAsync();

        await _publishEndpoint.Publish<IDoctorUpdatedEvent>(new
        {
            Id = doctor.Id,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            MiddleName = doctor.MiddleName,
            DateOfBirth = doctor.DateOfBirth,
            Specialization = doctor.Specialization,
            CareerStartYear = doctor.CareerStartYear,
            OfficeAddress = doctor.OfficeAddress,
            Status = doctor.Status
        });

        return Result.Success();
    }

    public async Task<Result<PagedResult<DoctorCardDto>>> GetFilteredDoctorsAsync(DoctorFilterDto filter)
    {
        var query = _doctorRepository.Query();

        query = query.Where(d => d.Status == "At work");

        var currentYear = DateTime.UtcNow.Year;

        var projectedQuery = query
            .ApplyFilters(filter) 
            .Select(d => new DoctorCardDto(
                d.PhotoPath,
                $"{d.FirstName} {d.LastName} {d.MiddleName}".Trim(),
                d.Specialization,
                currentYear - d.CareerStartYear + 1,
                d.OfficeAddress
            ));

        var pagedResult = await projectedQuery.ToPagedAsync(filter);

        return Result<PagedResult<DoctorCardDto>>.Success(pagedResult);
    }

    public async Task<Result<PagedResult<DoctorTableRowDto>>> GetDoctorsForReceptionistAsync(DoctorFilterDto filter)
    {
        var query = _doctorRepository.Query();

        var projectedQuery = query
            .ApplyFilters(filter)
            .Select(d => new DoctorTableRowDto(
                d.Id,
                $"{d.FirstName} {d.LastName} {d.MiddleName}".Trim(),
                d.Specialization,
                d.Status,
                d.DateOfBirth,
                d.OfficeAddress
            ));

        var pagedResult = await projectedQuery.ToPagedAsync(filter);

        return Result<PagedResult<DoctorTableRowDto>>.Success(pagedResult);
    }
}