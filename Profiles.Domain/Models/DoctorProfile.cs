namespace Profiles.Domain.Models;

public class DoctorProfile
{
    public Guid Id { get; set; }     //PK
    public string? AccountId { get; set; }      //FK
    public string? PhotoPath { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; } 
    public string Email { get; set; } = null!; 
    public string Specialization { get; set; } = null!;
    public int CareerStartYear { get; set; }
    public string OfficeAddress { get; set; } = null!;
    public string Status { get; set; } = "At work";
}