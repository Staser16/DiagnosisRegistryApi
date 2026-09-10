using DiagnosisRepositoryApi.DTOs;
using DiagnosisRepositoryApi.Handlers;

namespace DiagnosisRepositoryApi.GraphQL;

public class DiagnosesMutation
{
    public async Task<DTOs.Result<Object>> UpdateClinicalStatus(
        PatchClinicalStatusRequest request,
        [Service] ICommandHandler<PatchClinicalStatusRequest, DTOs.Result<Object>> clinicalPatchHandler)
    {
        var result = await clinicalPatchHandler.Handle(request);

        return result;
    }
}