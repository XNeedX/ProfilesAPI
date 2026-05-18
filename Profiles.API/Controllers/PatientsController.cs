using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Abstractions;
using Profiles.Application.DTOs;
using Profiles.Presentation.Responses;
using Profiles.Application.Mappings;

namespace Profiles.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ApiController
{
    private readonly IPatientService _patientService;
    private readonly IValidator<CreatePatientDto> _createValidator;

    public PatientsController(
        IPatientService patientService,
        IValidator<CreatePatientDto> createValidator)
    {
        _patientService = patientService;
        _createValidator = createValidator;
    }

    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm([FromBody] CreatePatientDto request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return HandleValidationFailure(validationResult.ToDictionary());

        var matchResult = await _patientService.FindMatchAsync(request);
        if (matchResult.IsFailure)
            return HandleFailure(matchResult.Error);

        var match = matchResult.Value;

        if (match != null)
        {
            var matchResponse = new PatientMatchResultDto(
                IsMatchFound: true,
                Message: "A similar profile has been found.",
                ExistingProfile: match.ToExistingProfileDto() 
            );

            return Ok(ApiResponse<PatientMatchResultDto>.Success(matchResponse));
        }

        var createResult = await _patientService.ConfirmAndCreateAsync(request, request.AccountId);
        if (createResult.IsFailure)
            return HandleFailure(createResult.Error);

        var successResponse = new PatientMatchResultDto(false, "Profile created", null);
        return Ok(ApiResponse<PatientMatchResultDto>.Success(successResponse, "Profile created successfully"));
    }

    [HttpPost("{profileId:guid}/link")]
    public async Task<IActionResult> LinkProfile(Guid profileId, [FromBody] string accountId)
    {
        if (string.IsNullOrWhiteSpace(accountId))
            return BadRequest(ApiResponse.Failure("Account ID cannot be empty."));

        var result = await _patientService.LinkExistingProfileAsync(profileId, accountId);
        return HandleResult(result, "Profile linked successfully");
    }

    [HttpPost("force-create")]
    public async Task<IActionResult> ForceCreate([FromBody] CreatePatientDto request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return HandleValidationFailure(validationResult.ToDictionary());

        var result = await _patientService.ConfirmAndCreateAsync(request, request.AccountId);

        if (result.IsSuccess)
        {
            var successResponse = new PatientMatchResultDto(false, "Profile created", null);
            return Ok(ApiResponse<PatientMatchResultDto>.Success(successResponse, "Profile created successfully"));
        }

        return HandleFailure(result.Error);
    }

    [HttpGet("account/{accountId}")]
    public async Task<IActionResult> GetProfile(string accountId)
    {
        var result = await _patientService.GetByAccountIdAsync(accountId);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    // [Authorize(Roles = "Doctor, Receptionist")]
    public async Task<IActionResult> GetProfileById(Guid id)
    {
        var result = await _patientService.GetPatientByIdAsync(id);
        return HandleResult(result);
    }
}