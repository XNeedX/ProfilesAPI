using FluentValidation;
using Profiles.Application.DTOs;
using Profiles.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Profiles.Presentation.Responses;
using Microsoft.AspNetCore.Authorization;

namespace Profiles.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ApiController
{
    private readonly IDoctorService _doctorService;
    private readonly IValidator<CreateDoctorDto> _createValidator;
    private readonly IValidator<UpdateDoctorDto> _updateValidator;

    public DoctorsController(
        IDoctorService doctorService,
        IValidator<CreateDoctorDto> createValidator,
        IValidator<UpdateDoctorDto> updateValidator)
    {
        _doctorService = doctorService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpPost]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorDto request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return HandleValidationFailure(validationResult.ToDictionary());

        var result = await _doctorService.CreateDoctorAsync(request);

        if (result.IsSuccess)
            return Ok(ApiResponse<Guid>.Success(result.Value!.Id, "Doctor profile created successfully"));

        return HandleFailure(result.Error);
    }

    [HttpGet]
    [Authorize(Roles = "Patient, Receptionist")]
    public async Task<IActionResult> GetDoctors([FromQuery] DoctorFilterDto filter)
    {
        var result = await _doctorService.GetFilteredDoctorsAsync(filter);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Patient, Doctor")]
    public async Task<IActionResult> GetDoctorById(Guid id)
    {
        var result = await _doctorService.GetDoctorProfileByIdAsync(id);
        return HandleResult(result);
    }

    [HttpGet("doctors")]
    [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> GetDoctorsForTable([FromQuery] DoctorFilterDto filter)
    {
        var result = await _doctorService.GetDoctorsForReceptionistAsync(filter);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Doctor, Receptionist")]
    public async Task<IActionResult> UpdateDoctorProfile(Guid id, [FromBody] UpdateDoctorDto request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return HandleValidationFailure(validationResult.ToDictionary());

        var result = await _doctorService.UpdateDoctorAsync(id, request);
        return HandleResult(result, "Profile updated successfully");
    }
}