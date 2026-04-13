using FluentValidation;
using InnoClinic.Profiles.Api.DTOs;
using InnoClinic.Profiles.Api.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Profiles.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReceptionistsController : ControllerBase
{
    private readonly IReceptionistService _receptionistService;
    private readonly IValidator<CreateReceptionistRequest> _createValidator;
    private readonly IValidator<ReceptionistUpdateRequest> _updateValidator;

    public ReceptionistsController(IReceptionistService receptionistService, IValidator<CreateReceptionistRequest> createValidator, IValidator<ReceptionistUpdateRequest> updateValidator)
    {
        _receptionistService = receptionistService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpPost]
    //[Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> CreateReceptionist([FromBody] CreateReceptionistRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }

        try
        {
            var receptionist = await _receptionistService.CreateReceptionistAsync(request);
            return Ok(new { Message = "Receptionist profile created successfully", ReceptionistId = receptionist.Id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet]
    //[Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> GetAllReceptionists()
    {
        var receptionists = await _receptionistService.GetAllAsync();
        return Ok(receptionists);
    }

    [HttpGet("{id}")]
    //[Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> GetReceptionist(Guid id)
    {
        try
        {
            var receptionist = await _receptionistService.GetReceptionistAsync(id);
            return Ok(receptionist);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    //[Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> UpdateReceptionist(Guid id, [FromBody] ReceptionistUpdateRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }
        try
        {
            await _receptionistService.UpdateReceptionistAsync(id, request);
            return Ok(new { Message = "Receptionist profile updated successfully", ReceptionistId = id });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    //[Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> DeleteReceptionist(Guid id)
    {
        try
        {
            await _receptionistService.DeleteReceptionistAsync(id);
            return Ok(new { Message = "Receptionist profile deleted successfully", ReceptionistId = id });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }
}