using DiagnosisRepositoryApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using DiagnosisRepositoryApi.DTOs;
using DiagnosisRepositoryApi.Entities;
using DiagnosisRepositoryApi.Handlers;

namespace DiagnosisRepositoryApi.Controllers;

[ApiController]
[Route("api/record")]
public class CommandRecordController(
    ICommandHandler<RegisterRequest, DTOs.Result<RegisterResponse>> RegistrationHandler,
    ICommandHandler<PatchClinicalStatusRequest, DTOs.Result<Object>> ClinicalPatchHandler) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<DTOs.Result<RegisterResponse>>> Register(RegisterRequest request)
    {
        var response = await RegistrationHandler.Handle(request);

        if (response.IsGood)
        {
            return CreatedAtAction(
                nameof(QueryRecordController.GetRecord),
                "QueryRecord",
                new
                {
                    Provider = response.Value!.Provider,
                    sourceId = response.Value.SourceId
                },
                response.Value);
        }

        string Errors = "";

        foreach (var errors in response.Error!)
        {
            Errors += errors.Code + ": " + errors.Description + ", ";
        }

        return Problem(detail: Errors,
            statusCode: StatusCodes.Status400BadRequest,
            title: "Validation error");
    }

[HttpPost("Rejestr")]
    public IActionResult Rejestr(RejestrRequest request)
    {
        if(request.SourceId == Guid.Empty || string.IsNullOrWhiteSpace(request.Code))
        {
            return BadRequest();
        }

        if (request.Attempt < 3)
        {
            return StatusCode(503);
        }

        return Accepted();
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateClinicalStatus(PatchClinicalStatusRequest request)
    {
        var result = await ClinicalPatchHandler.Handle(request);

        if (result.IsGood)
            return NoContent();
        else
            return BadRequest(result.Error);

    }

}
