using Microsoft.AspNetCore.Mvc;
using DiagnosisRepositoryApi.DTOs;
using DiagnosisRepositoryApi.Entities;
using DiagnosisRepositoryApi.Handlers;

namespace DiagnosisRepositoryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QueryRecordController(
    IQueryHandler<object, List<CollectionResponse>> DiagnoseHandler,
    IQueryHandler<ReadRecognitionRequest, DiagnosisRepositoryApi.DTOs.Result<List<ReadRecognitionResponse>>> DiagnosePatientHandler
) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CollectionResponse>> GetAllDiagnosis()
    {
        var result = await DiagnoseHandler.Handle(new object());
        return Ok(result);
    }

    [HttpGet("Patient")]
    public async Task<ActionResult<List<ReadRecognitionRequest>>> GetPatientDiagnosis([FromQuery] ReadRecognitionRequest request)
    {
        var result = await DiagnosePatientHandler.Handle(request);
        if(result.IsGood)
            return Ok(result.Value);
        else
            return BadRequest(result.Error);

    }

    [HttpGet("{Provider}/{SourceId}")]
    public IActionResult GetRecord(Provider Provider, Guid SourceId)
    {
        return Ok();
    }
}
