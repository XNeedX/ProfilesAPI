namespace Profiles.Domain.Models;

public class Receptionist
{
    public Guid Id { get; set; } // PK
    public string? AccountId { get; set; } // FK 
    public string? PhotoPath { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string Email { get; set; } = null!;
    public string OfficeAddress { get; set; } = null!;
}