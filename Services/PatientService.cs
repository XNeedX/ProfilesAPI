using Microsoft.EntityFrameworkCore;
using InnoClinic.Profiles.Api.Abstractions;
using InnoClinic.Profiles.Api.DTOs;
using InnoClinic.Profiles.Api.Models;

namespace InnoClinic.Profiles.Api.Services;

public class PatientService : IPatientService
{
    private readonly ProfilesDbContext _context;
    public PatientService(ProfilesDbContext context) => _context = context;

    public async Task UpdateAsync(Guid id, PatientProfileRegistrationRequest request)
    {
        var existing = await _context.Profiles.FindAsync(id);
        if (existing == null) throw new KeyNotFoundException("Profile not found");

        existing.FirstName = request.FirstName;
        existing.LastName = request.LastName;
        existing.MiddleName = request.MiddleName;
        existing.PhoneNumber = request.PhoneNumber;
        existing.DateOfBirth = request.DateOfBirth;
        existing.PhotoPath = request.Photo;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var profile = await _context.Profiles.FindAsync(id);
        if (profile != null)
        {
            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<PatientProfile> CreateInitialAsync(string email)
    {
        var profile = new PatientProfile
        {
            Id = Guid.NewGuid(),
            IsEmailVerified = false,   
            IsLinkedToAccount = false  
        };

        _context.Profiles.Add(profile);
        await _context.SaveChangesAsync();
        return profile;
    }

    public async Task<PatientProfile?> FindMatchAsync(PatientProfileRegistrationRequest request)
    {
        var candidates = await _context.Profiles.ToListAsync();
        var bestMatch = candidates.FirstOrDefault(p => CalculateMatchScore(p, request) >= 13);

        if (bestMatch == null)
            return null; 

        if (bestMatch.IsLinkedToAccount)
        {
            throw new ArgumentException("A patient with this personal data is already registered in the system.");
        }

        return bestMatch;
    }

    private int CalculateMatchScore(PatientProfile existing, PatientProfileRegistrationRequest incoming)
    {
        int score = 0;
        if (string.Equals(existing.FirstName, incoming.FirstName, StringComparison.OrdinalIgnoreCase)) score += 5;
        if (string.Equals(existing.LastName, incoming.LastName, StringComparison.OrdinalIgnoreCase)) score += 5;
        if (existing.MiddleName != null && string.Equals(existing.MiddleName, incoming.MiddleName, StringComparison.OrdinalIgnoreCase)) score += 5;
        if (existing.DateOfBirth.Date == incoming.DateOfBirth.Date) score += 3;
        return score;
    }

    public async Task<PatientProfile> ConfirmAndCreateAsync(PatientProfileRegistrationRequest request, string accountId)
    {
        var profile = new PatientProfile
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            PhoneNumber = request.PhoneNumber,
            DateOfBirth = request.DateOfBirth,
            PhotoPath = request.Photo,

            IsEmailVerified = request.IsEmailVerified,
            IsLinkedToAccount = true,
            AccountId = accountId 
        };

        _context.Profiles.Add(profile);
        await _context.SaveChangesAsync();
        return profile;
    }

    public async Task LinkExistingProfileAsync(Guid profileId, string accountId)
    {
        var profile = await _context.Profiles.FindAsync(profileId);
        if (profile != null)
        {
            profile.IsLinkedToAccount = true;
            profile.AccountId = accountId;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<PatientProfileViewByPatientResponse?> GetByAccountIdAsync(string accountId)
    {
        var profile = await _context.Profiles
            .FirstOrDefaultAsync(p => p.AccountId == accountId && p.IsLinkedToAccount);

        if (profile == null)
            return null;

        return new PatientProfileViewByPatientResponse(
            Photo: profile.PhotoPath,
            FirstName: profile.FirstName,
            LastName: profile.LastName,
            MiddleName: profile.MiddleName,
            PhoneNumber: profile.PhoneNumber,
            DateOfBirth: profile.DateOfBirth
        );
    }

    public async Task<ProfileViewResponse?> GetPatientByIdAsync(Guid id)
    {
        var profile = await _context.Profiles.FindAsync(id);

        if (profile == null)
            return null;

        return new ProfileViewResponse(
            Photo: profile.PhotoPath,
            FirstName: profile.FirstName,
            LastName: profile.LastName,
            MiddleName: profile.MiddleName,
            PhoneNumber: profile.PhoneNumber,
            DateOfBirth: profile.DateOfBirth
        );
    }
}