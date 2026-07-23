using Profiles.Application.DTOs;
using Profiles.Domain.Models;
using Riok.Mapperly.Abstractions;

namespace Profiles.Application.Mappings;

[Mapper]
public static partial class ReceptionistMapper
{
    [MapperIgnoreTarget(nameof(Receptionist.Id))]
    [MapperIgnoreTarget(nameof(Receptionist.AccountId))]
    public static partial Receptionist ToEntity(this CreateReceptionistRequest request);

    [MapperIgnoreTarget(nameof(Receptionist.Id))]
    [MapperIgnoreTarget(nameof(Receptionist.AccountId))]
    public static partial void UpdateEntity(this UpdateReceptionistDto request, Receptionist receptionist);

    public static partial ReceptionistProfileDto ToViewResponse(this Receptionist receptionist);
}