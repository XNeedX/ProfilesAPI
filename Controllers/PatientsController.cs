using InnoClinic.Profiles.Api.Abstractions;
using InnoClinic.Profiles.Api.DTOs;
using InnoClinic.Profiles.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

[ApiController]
[Route("api/[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly IPatientService _patientService;
    public ProfilesController(IPatientService profileService)
        => _patientService = profileService;

    [HttpPost("confirm")]
    public async Task<ActionResult<PatientProfileMatchResponse>> Confirm([FromBody] PatientProfileRegistrationRequest request)
    {
        try
        {
            var match = await _patientService.FindMatchAsync(request);

            if (match != null)
            {
                return Ok(new PatientProfileMatchResponse(
                    IsMatchFound: true,
                    Message: "A similar profile has been found...",
                    ExistingProfile: new ExistingProfileDto(match.Id, match.FirstName, match.LastName, match.MiddleName, match.DateOfBirth)
                ));
            }

            // Совпадений вообще нет - создаем новый
            await _patientService.ConfirmAndCreateAsync(request, request.AccountId);
            return Ok(new PatientProfileMatchResponse(false, "Profile created", null));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("link/{profileId}")]
    public async Task<IActionResult> LinkProfile(Guid profileId, [FromBody] string accountId)
    {
        await _patientService.LinkExistingProfileAsync(profileId, accountId);
        return Ok(new { Message = "Profile linked successfully" });
    }

    [HttpPost("force-create")]
    public async Task<ActionResult> ForceCreate([FromBody] PatientProfileRegistrationRequest request)
    {
        await _patientService.ConfirmAndCreateAsync(request, request.AccountId);
        return Ok(new PatientProfileMatchResponse(false, "Profile created", null));
    }

    [HttpGet("account/{accountId}")]
    // [Authorize] 
    public async Task<ActionResult<PatientProfileViewByPatientResponse>> GetProfile(string accountId)
    {
        var profile = await _patientService.GetByAccountIdAsync(accountId);

        if (profile == null)
        {
            return NotFound(new { Message = "Profile not found for the given account." });
        }

        return Ok(profile);
    }

    [HttpGet("{id:guid}")]
    // [Authorize(Roles = "Doctor, Receptionist")]
    public async Task<ActionResult<ProfileViewResponse>> GetProfileById(Guid id)
    {
        var profile = await _patientService.GetPatientByIdAsync(id);

        if (profile == null)
            return NotFound(new { Message = "Patient profile not found." });

        return Ok(profile);
    }
}