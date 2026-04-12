using FluentValidation;
using InnoClinic.Profiles.Api.DTOs;
using InnoClinic.Profiles.Api.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;
    private readonly IValidator<DoctorProfileRegistrationRequest> _createValidator;
    private readonly IValidator<DoctorUpdateRequest> _updateValidator;

    public DoctorsController(IDoctorService doctorService, IValidator<DoctorProfileRegistrationRequest> createValidator, IValidator<DoctorUpdateRequest> updateValidator)
    {
        _doctorService = doctorService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpPost]
    // [Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> CreateDoctor([FromBody] DoctorProfileRegistrationRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }
        try
        {
            var doctor = await _doctorService.CreateDoctorAsync(request);
            return Ok(new { Message = "Doctor profile created successfully", DoctorId = doctor.Id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet]
    // [Authorize(Roles = "Patient, Receptionist")]
    public async Task<ActionResult<IEnumerable<DoctorCardResponse>>> GetDoctors([FromQuery] DoctorFilterRequest filter)
    {
        var doctors = await _doctorService.GetFilteredDoctorsAsync(filter);
        return Ok(doctors);
    }

    [HttpGet("{id:guid}")]
    // Authorize(Roles = "Doctor")]
    public async Task<ActionResult<DoctorViewByDoctorResponse>> GetDoctorById(Guid id)
    {
        var doctor = await _doctorService.GetDoctorProfileByIdAsync(id);
        if (doctor == null)
        {
            return NotFound(new { Message = "Doctor profile not found." });
        }
        return Ok(doctor);
    }

    [HttpGet("doctors")]
    // [Authorize(Roles = "Receptionist")]
    public async Task<ActionResult<IEnumerable<DoctorTableRowResponse>>> GetDoctorsForTable([FromQuery] DoctorFilterRequest filter)
    {
        var tableRows = await _doctorService.GetDoctorsForReceptionistAsync(filter);
        return Ok(tableRows);
    }

    [HttpPut("{id:guid}")]
    // [Authorize(Roles = "Doctor, Receptionist")]
    public async Task<IActionResult> UpdateDoctorProfile(Guid id, [FromBody] DoctorUpdateRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary()); 
        }

        try
        {
            await _doctorService.UpdateDoctorAsync(id, request);
            return Ok(new { Message = "Profile updated successfully" }); 
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }
}