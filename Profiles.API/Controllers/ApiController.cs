using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Abstractions;
using Profiles.Presentation.Responses; 

namespace Profiles.Presentation.Controllers;

[ApiController]
public abstract class ApiController : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result, string? successMessage = null)
    {
        if (result.IsSuccess)
        {
            return Ok(ApiResponse<T>.Success(result.Value, successMessage));
        }

        return HandleFailure(result.Error);
    }

    protected IActionResult HandleResult(Result result, string? successMessage = null)
    {
        if (result.IsSuccess)
        {
            return Ok(ApiResponse.Success(successMessage));
        }

        return HandleFailure(result.Error);
    }

    protected IActionResult HandleFailure(Error error)
    {
        var apiResponse = ApiResponse.Failure(error.Message);

        return error.Type switch
        {
            ErrorType.NotFound => NotFound(apiResponse),      
            ErrorType.Conflict => Conflict(apiResponse),     
            ErrorType.Forbidden => StatusCode(403, apiResponse), 
            ErrorType.Validation => BadRequest(apiResponse), 
            _ => BadRequest(apiResponse)                      
        };
    }

    protected IActionResult HandleValidationFailure(IDictionary<string, string[]> validationErrors)
    {
        var errors = validationErrors.SelectMany(kv => kv.Value).ToList();

        var apiResponse = ApiResponse.Failure(errors, "Ошибка валидации данных");
        return BadRequest(apiResponse);
    }
}