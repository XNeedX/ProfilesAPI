using Profiles.Domain.Models;

namespace Profiles.Application.DTOs;

public record DoctorFilterDto(
    string? SearchName = null,
    string? Specialization = null,
    string? OfficeAddress = null,
    int Page = 1,
    int PageSize = 10
) : PageParams(Page, PageSize);
