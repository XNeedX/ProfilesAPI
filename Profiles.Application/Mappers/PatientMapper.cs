using Profiles.Application.DTOs;
using Profiles.Domain.Models;
using Riok.Mapperly.Abstractions;

namespace Profiles.Application.Mappings;

[Mapper]
public static partial class PatientMapper
{
    [MapperIgnoreTarget(nameof(PatientProfile.Id))]
    public static partial PatientProfile ToEntity(this CreatePatientDto request);

    [MapperIgnoreTarget(nameof(PatientProfile.Id))]
    [MapperIgnoreTarget(nameof(PatientProfile.AccountId))]
    [MapperIgnoreTarget(nameof(PatientProfile.IsEmailVerified))]
    [MapperIgnoreTarget(nameof(PatientProfile.IsLinkedToAccount))]
    public static partial void UpdateEntity(this CreatePatientDto request, PatientProfile profile);

    public static partial PatientProfileDto ToPatientViewResponse(this PatientProfile profile);

    public static partial PatientProfileDto ToViewResponse(this PatientProfile profile);

    public static partial ExistingProfileDto ToExistingProfileDto(this PatientProfile profile);
}