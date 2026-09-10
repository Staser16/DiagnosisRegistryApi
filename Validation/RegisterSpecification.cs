using Microsoft.EntityFrameworkCore;
using DiagnosisRepositoryApi.Data;
using DiagnosisRepositoryApi.DTOs;
using DiagnosisRepositoryApi.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DiagnosisRepositoryApi.Validation;

public class RegisterSpecification(ValueSet valueSet, MyDbContext context)
{
    public ISpecification<RegisterRequest> NotFutureDate()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return new Specification<RegisterRequest>(x=>x.CreationDate <= today);
    }

    public ISpecification<RegisterRequest> ICD10CodesValidation()
    {
        return new Specification<RegisterRequest>(x =>
        valueSet.Compose.Include
            .Any(Include => Include.Concept
            .OfType<DiagnosesConcept>()
            .Any(Concept => Concept.Code == x.Code))
        );
    }

    public ISpecification<RegisterRequest> ICD10SystemValidation()
    {
        return new Specification<RegisterRequest>(x =>
        valueSet.Compose.Include
            .Any(Include => Include.System == x.CodingSystem)
        );
    }

    public ISpecification<RegisterRequest> AgeOrDate()
    {
        return new Specification<RegisterRequest>(x => x.Age.HasValue != x.StartDate.HasValue);
    }

    public ISpecification<RegisterRequest> PatientExist()
    {
        return new Specification<RegisterRequest>(x => context.Patients.Any(p => p.Id == x.PatientId));
    }
}
