namespace InnoClinic.Profiles.Api.DTOs;

public sealed class DoctorUpdateRequest
{
    public string? Photo { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Specialization { get; set; }
    public string Office { get; set; }
    public int CareerStartYear { get; set; }
    public string Status { get; set; }
}