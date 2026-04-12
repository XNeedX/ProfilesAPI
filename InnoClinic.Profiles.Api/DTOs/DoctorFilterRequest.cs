namespace InnoClinic.Profiles.Api.DTOs;

public class DoctorFilterRequest
{
    public string? SearchName { get; set; }
    public string? Specialization { get; set; }
    public string? OfficeAddress { get; set; }
}
