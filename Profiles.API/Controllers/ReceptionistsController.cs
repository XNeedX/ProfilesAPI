using FluentValidation;
using Profiles.Application.DTOs;
using Profiles.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Profiles.Domain.Models; // Для PageParams
using Profiles.Presentation.Responses; // Ваш namespace с ApiResponse

namespace Profiles.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReceptionistsController : ApiController
{
    private readonly IReceptionistService _receptionistService;
    private readonly IValidator<CreateReceptionistRequest> _createValidator;
    private readonly IValidator<UpdateReceptionistDto> _updateValidator;

    public ReceptionistsController(
        IReceptionistService receptionistService,
        IValidator<CreateReceptionistRequest> createValidator,
        IValidator<UpdateReceptionistDto> updateValidator)
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
            return HandleValidationFailure(validationResult.ToDictionary());

        var result = await _receptionistService.CreateReceptionistAsync(request);

        if (result.IsSuccess)
            return Ok(ApiResponse<Guid>.Success(result.Value!.Id, "Receptionist profile created successfully"));

        return HandleFailure(result.Error);
    }

    [HttpGet]
    //[Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> GetAllReceptionists([FromQuery] PageParams pageParams) // Добавили пагинацию
    {
        var result = await _receptionistService.GetAllPagedAsync(pageParams); 
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    //[Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> GetReceptionist(Guid id)
    {
        var result = await _receptionistService.GetReceptionistAsync(id);
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    //[Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> UpdateReceptionist(Guid id, [FromBody] UpdateReceptionistDto request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return HandleValidationFailure(validationResult.ToDictionary());

        var result = await _receptionistService.UpdateReceptionistAsync(id, request);
        return HandleResult(result, "Receptionist profile updated successfully");
    }

    [HttpDelete("{id}")]
    //[Authorize(Roles = "Receptionist")]
    public async Task<IActionResult> DeleteReceptionist(Guid id)
    {
        var result = await _receptionistService.DeleteReceptionistAsync(id);
        return HandleResult(result, "Receptionist profile deleted successfully");
    }
}