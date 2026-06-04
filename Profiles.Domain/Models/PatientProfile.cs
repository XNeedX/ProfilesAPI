namespace Profiles.Domain.Models;

public class PatientProfile
{
    public Guid Id { get; set; } // PK
    public string? AccountId { get; set; } // FK
    public string? PhotoPath { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public bool IsEmailVerified { get; set; }
    public bool IsLinkedToAccount { get; set; }
}

