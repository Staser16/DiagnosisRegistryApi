using DiagnosisRepositoryApi.DTOs;
using DiagnosisRepositoryApi.Handlers;

namespace DiagnosisRepositoryApi.GraphQL;

public class DiagnosesQuery
{
    public async Task<DTOs.Result<List<ReadRecognitionResponse>>> GetPatientDiagnosis(ReadRecognitionRequest request,
    [Service] IQueryHandler<ReadRecognitionRequest, DTOs.Result<List<ReadRecognitionResponse>>> DiagnosePatientHandler)
    {
        var result = await DiagnosePatientHandler.Handle(request);

        return result;
    }
}
