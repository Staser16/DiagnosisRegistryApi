using DiagnosisRepositoryApi.Data;
using DiagnosisRepositoryApi.DTOs;
using DiagnosisRepositoryApi.Handlers;
using DiagnosisRepositoryApi.Validation;

namespace DiagnosisRepositoryApi.Decorators.ClinicalStatusPatch;

public class ValidationDecorator(MyDbContext context, ICommandHandler<PatchClinicalStatusRequest, DTOs.Result<Object>> handler) : ICommandHandler<PatchClinicalStatusRequest, DTOs.Result<Object>>
{
    private readonly ICommandHandler<PatchClinicalStatusRequest, DTOs.Result<Object>> _handler = handler;
    public Task<DTOs.Result<object>> Handle(PatchClinicalStatusRequest command)
    {
        var diagnosis = context.Diagnoses.Where(x => x.SourceId == command.SourceId && x.Provider == command.Provider).FirstOrDefault();

        List<DTOs.Error> Errors = new();

        if(diagnosis is null)
        {
            Errors.Add(new DTOs.Error.NotFoundDiagnosisError(command.SourceId, command.Provider));
            return Task.FromResult(DTOs.Result<Object>.Fail(Errors));
        }

        var Allowed = new PatchClinicalStatusSpecification(diagnosis).Allowed();

        if(!Allowed.isSatisfiedBy(command))
            Errors.Add(new DTOs.Error.ValidationError("Can not change status from healed to active"));

        if(Errors.Count > 0)
            return Task.FromResult(DTOs.Result<Object>.Fail(Errors));

        return _handler.Handle(command);
    }
}
