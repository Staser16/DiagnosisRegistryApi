using DiagnosisRepositoryApi.DTOs;
using DiagnosisRepositoryApi.Handlers;
using DiagnosisRepositoryApi.Validation;

namespace DiagnosisRepositoryApi.Decorators.Recognition;

public class ValidationDecorator(IQueryHandler<ReadRecognitionRequest, DTOs.Result<List<ReadRecognitionResponse>>> handler) : IQueryHandler<ReadRecognitionRequest, DTOs.Result<List<ReadRecognitionResponse>>>
{
    private readonly IQueryHandler<ReadRecognitionRequest, DTOs.Result<List<ReadRecognitionResponse>>> _handler = handler;
    public async Task<DTOs.Result<List<ReadRecognitionResponse>>> Handle(ReadRecognitionRequest command)
    {

        var XOR = new RecognitionSpecification().XOR();

        List<DTOs.Error> Errors = new();

        if (!XOR.isSatisfiedBy(command))
            Errors.Add(new DTOs.Error.ValidationError("You should give Pesel or patient id, and never all of them at the same time"));

        if(Errors.Count>0)
            return DTOs.Result<List<ReadRecognitionResponse>>.Fail(Errors);

        return await _handler.Handle(command);
    }
}
