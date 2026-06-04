namespace Profiles.Application.DTOs;

public sealed class UpdateDoctorDto
{
    public string? PhotoPath { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? MiddleName { get; init; }
    public DateTime DateOfBirth { get; init; }
    public string Specialization { get; init; }
    public string OfficeAddress { get; init; }
    public int CareerStartYear { get; init; }
    public string Status { get; init; }
}