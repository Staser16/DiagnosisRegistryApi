using DiagnosisRepositoryApi.Data;
using DiagnosisRepositoryApi.DTOs;
using DiagnosisRepositoryApi.Entities;

namespace DiagnosisRepositoryApi.Validation;

public class PatchClinicalStatusSpecification(Record diagnosis)
{
    public ISpecification<PatchClinicalStatusRequest> Allowed()
    {
        return new Specification<PatchClinicalStatusRequest>(x=>!(x.ClinicalStatus == ClinicalStatus.Active && diagnosis.ClinicalStatus == ClinicalStatus.Healed));
    }

}
