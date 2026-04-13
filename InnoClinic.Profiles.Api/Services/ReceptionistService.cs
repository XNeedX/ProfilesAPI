using Microsoft.EntityFrameworkCore;
using InnoClinic.Profiles.Api.Abstractions;
using InnoClinic.Profiles.Api.DTOs;
using InnoClinic.Profiles.Api.Models;

namespace InnoClinic.Profiles.Api.Services;

public class ReceptionistService : IReceptionistService
{
    private readonly ProfilesDbContext _context;
    private readonly IPasswordGenerator _passwordGenerator;
    private readonly IEmailService _emailService;

    public ReceptionistService(ProfilesDbContext context, IPasswordGenerator passwordGenerator, IEmailService emailService)
    {
        _context = context;
        _passwordGenerator = passwordGenerator;
        _emailService = emailService;
    }

    public async Task<Receptionist> CreateReceptionistAsync(CreateReceptionistRequest request)
    {
        var emailExists = await _context.Receptionists.AnyAsync(r => r.Email == request.Email);
        if (emailExists)
        {
            throw new ArgumentException("User with this email already exists");
        }

        var generatedPassword = _passwordGenerator.GeneratePassword();

        var receptionist = new Receptionist
        {
            Id = Guid.NewGuid(),
            PhotoPath = request.Photo,
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            Email = request.Email,
            OfficeAddress = request.Office
        };

        _context.Receptionists.Add(receptionist);
        await _context.SaveChangesAsync();

        await _emailService.SendCredentialsAsync(receptionist.Email, generatedPassword);

        return receptionist;
    }

    public async Task<ReceptionistViewRequest> GetReceptionistAsync(Guid id)
    {
        var receptionist = await _context.Receptionists.FindAsync(id);
        if (receptionist == null)
        {
            throw new KeyNotFoundException("Receptionist not found");
        }
        return new ReceptionistViewRequest(
            receptionist.PhotoPath,
            receptionist.FirstName,
            receptionist.LastName,
            receptionist.MiddleName,
            receptionist.OfficeAddress
        );
    }

    public async Task UpdateReceptionistAsync(Guid id, ReceptionistUpdateRequest request)
    {
        var profile = await _context.Receptionists.FindAsync(id);
        if (profile == null) throw new KeyNotFoundException("Receptionist not found");
        profile.FirstName = request.FirstName;
        profile.LastName = request.LastName;
        profile.MiddleName = request.MiddleName;
        profile.PhotoPath = request.Photo;
        profile.OfficeAddress = request.Office;
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ReceptionistViewRequest>> GetAllAsync()
    {
        var receptionists = await _context.Receptionists.ToListAsync();
        return receptionists.Select(r => new ReceptionistViewRequest(
            r.PhotoPath,
            r.FirstName,
            r.LastName,
            r.MiddleName,
            r.OfficeAddress
        ));
    }

    public async Task DeleteReceptionistAsync(Guid id)
    {
        var profile = await _context.Receptionists.FindAsync(id);
        if (profile != null)
        {
            _context.Receptionists.Remove(profile);
            await _context.SaveChangesAsync();
        }
    }
}