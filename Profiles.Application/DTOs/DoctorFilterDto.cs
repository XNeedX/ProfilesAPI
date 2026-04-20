using Profiles.Domain.Models;

namespace Profiles.Application.DTOs;

public class DoctorFilterDto : PageParams
{
    public string? SearchName { get; set; }
    public string? Specialization { get; set; }
    public string? OfficeAddress { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
