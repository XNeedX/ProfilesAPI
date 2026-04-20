using Profiles.Application.DTOs;
using Profiles.Domain.Models;
using Riok.Mapperly.Abstractions;

namespace Profiles.Application.Mappings;

[Mapper]
public static partial class DoctorMapper
{
    [MapperIgnoreTarget(nameof(DoctorProfile.Id))]
    [MapperIgnoreTarget(nameof(DoctorProfile.AccountId))]
    public static partial DoctorProfile ToEntity(this CreateDoctorDto request);

    [MapperIgnoreTarget(nameof(DoctorProfile.Id))]
    [MapperIgnoreTarget(nameof(DoctorProfile.AccountId))]
    [MapperIgnoreTarget(nameof(DoctorProfile.Email))]
    public static partial void UpdateEntity(this UpdateDoctorDto request, DoctorProfile doctor);

    public static partial DoctorDetailsDto ToDoctorViewResponse(this DoctorProfile doctor);
    public static partial DoctorAdminDto ToAdminViewResponse(this DoctorProfile doctor);
}