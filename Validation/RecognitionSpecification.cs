using DiagnosisRepositoryApi.DTOs;

namespace DiagnosisRepositoryApi.Validation;

public class RecognitionSpecification
{
    public ISpecification<ReadRecognitionRequest> XOR()
    {
        return new Specification<ReadRecognitionRequest>(x=>!string.IsNullOrWhiteSpace(x.Pesel) != x.PatientId.HasValue);
    }
}
