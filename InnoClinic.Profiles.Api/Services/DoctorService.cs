using InnoClinic.Profiles.Api.Abstractions;
using InnoClinic.Profiles.Api.DTOs;
using InnoClinic.Profiles.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace InnoClinic.Profiles.Api.Services;

public class DoctorService : IDoctorService
{
    private readonly ProfilesDbContext _context;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IEmailService _emailService;

    public DoctorService(ProfilesDbContext context, IPasswordGenerator passwordGenerator, IEmailService emailService)
    {
        _context = context;
        _passwordGenerator = passwordGenerator;
        _emailService = emailService;
    }

    public async Task<DoctorProfile> CreateDoctorAsync(DoctorProfileRegistrationRequest request)
    {
        var emailExists = await _context.Doctors.AnyAsync(d => d.Email == request.Email);
        if (emailExists)
        {
            throw new ArgumentException("User with this email already exists");
        }

        var generatedPassword = _passwordGenerator.GeneratePassword();

        var doctor = new DoctorProfile
        {
            Id = Guid.NewGuid(),
            PhotoPath = request.Photo,
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            DateOfBirth = request.DateOfBirth,
            Email = request.Email,
            Specialization = request.Specialization,
            OfficeAddress = request.Office,
            CareerStartYear = request.CareerStartYear,
            Status = request.Status
        };

        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();

        await _emailService.SendCredentialsAsync(doctor.Email, generatedPassword);

        return doctor;
    }

    public async Task<DoctorViewByDoctorResponse?> GetDoctorProfileByIdAsync(Guid id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null)
            return null;

        return new DoctorViewByDoctorResponse(
            doctor.PhotoPath,
            doctor.FirstName,
            doctor.LastName,
            doctor.MiddleName,
            doctor.DateOfBirth,
            doctor.Specialization,
            doctor.OfficeAddress,
            doctor.CareerStartYear
        );
    }

    public async Task<DoctorViewByAdminResponse?> GetDoctorProfileForAdminAsync(Guid id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null) return null;

        return new DoctorViewByAdminResponse(
            doctor.PhotoPath, doctor.FirstName, doctor.LastName, doctor.MiddleName,
            doctor.DateOfBirth, doctor.Specialization, doctor.OfficeAddress, doctor.CareerStartYear,
            doctor.Status
        );
    }

    public async Task<IEnumerable<DoctorCardResponse>> GetFilteredDoctorsAsync(DoctorFilterRequest filter)
    {
        var query = _context.Doctors.AsQueryable();

        query = query.Where(d => d.Status == "At work");

        if (!string.IsNullOrWhiteSpace(filter.SearchName))
        {
            var search = filter.SearchName.ToLower();
            query = query.Where(d =>
                (d.FirstName + " " + d.LastName + " " + d.MiddleName).ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(filter.Specialization))
        {
            query = query.Where(d => d.Specialization == filter.Specialization);
        }

        if (!string.IsNullOrWhiteSpace(filter.OfficeAddress))
        {
            query = query.Where(d => d.OfficeAddress == filter.OfficeAddress);
        }

        var currentYear = DateTime.UtcNow.Year;

        var result = await query
            .Select(d => new DoctorCardResponse(
                d.PhotoPath,
                $"{d.FirstName} {d.LastName} {d.MiddleName}".Trim(),
                d.Specialization,
                currentYear - d.CareerStartYear + 1,
                d.OfficeAddress
            ))
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<DoctorTableRowResponse>> GetDoctorsForReceptionistAsync(DoctorFilterRequest filter)
    {
        var query = _context.Doctors.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchName))
        {
            var search = filter.SearchName.ToLower();
            query = query.Where(d =>
                (d.FirstName + " " + d.LastName + " " + d.MiddleName).ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(filter.Specialization))
            query = query.Where(d => d.Specialization == filter.Specialization);

        if (!string.IsNullOrWhiteSpace(filter.OfficeAddress))
            query = query.Where(d => d.OfficeAddress == filter.OfficeAddress);

        var result = await query
            .Select(d => new DoctorTableRowResponse(
                d.Id,
                $"{d.FirstName} {d.LastName} {d.MiddleName}".Trim(),
                d.Specialization,
                d.Status,
                d.DateOfBirth,
                d.OfficeAddress
            )).ToListAsync();

        return result;
    }

    public async Task UpdateDoctorAsync(Guid id, DoctorUpdateRequest request)
    {
        var doctor = await _context.Doctors.FindAsync(id);

        if (doctor == null)
            throw new KeyNotFoundException("Doctor profile not found");

        doctor.PhotoPath = request.Photo;
        doctor.FirstName = request.FirstName;
        doctor.LastName = request.LastName;
        doctor.MiddleName = request.MiddleName;
        doctor.DateOfBirth = request.DateOfBirth;
        doctor.Specialization = request.Specialization;
        doctor.OfficeAddress = request.Office;
        doctor.CareerStartYear = request.CareerStartYear;
        doctor.Status = request.Status;

        await _context.SaveChangesAsync();
    }
}