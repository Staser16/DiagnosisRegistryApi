using Microsoft.EntityFrameworkCore;
using DiagnosisRepositoryApi.Data;
using DiagnosisRepositoryApi.DTOs;
using DiagnosisRepositoryApi.Entities;
using DiagnosisRepositoryApi.Handlers;
using DiagnosisRepositoryApi.Validation;
namespace DiagnosisRepositoryApi.Decorators.Register;

public class ValidationDecorator(ValueSet valueSet, MyDbContext context, ICommandHandler<RegisterRequest, DTOs.Result<RegisterResponse>> handler) : ICommandHandler<RegisterRequest, DTOs.Result<RegisterResponse>>
{
    readonly ValueSet _valueSet = valueSet;
    readonly ICommandHandler<RegisterRequest, DTOs.Result<RegisterResponse>> _handler = handler;

    public async Task<DTOs.Result<RegisterResponse>> Handle(RegisterRequest command)
    {
        var ValidateICDCodes = new RegisterSpecification(_valueSet, context).ICD10CodesValidation();
        var ValidateSystem = new RegisterSpecification(_valueSet, context).ICD10SystemValidation();
        var ValidateCreationDate = new RegisterSpecification(_valueSet, context).NotFutureDate();
        var AgeOrDate = new RegisterSpecification(_valueSet, context).AgeOrDate();
        var PatientExists = new RegisterSpecification(_valueSet, context).PatientExist();

        List<DTOs.Error> Errors = new();

        if(!ValidateICDCodes.isSatisfiedBy(command))
            Errors.Add(new DTOs.Error.ValidationError("Supplied Code Is not valid for this valueset"));
        if(!ValidateSystem.isSatisfiedBy(command))
            Errors.Add(new DTOs.Error.ValidationError("Only DIAGNOSIS System is supported"));
        if(!ValidateCreationDate.isSatisfiedBy(command))
            Errors.Add(new DTOs.Error.ValidationError("Creation date is not suppossed to be in the future."));
        if(!AgeOrDate.isSatisfiedBy(command))
            Errors.Add(new DTOs.Error.ValidationError("You should give Age or Start date, not both at the same time"));
        if(!PatientExists.isSatisfiedBy(command))
            Errors.Add(new DTOs.Error.ValidationError($"There are no patient with the following Id: {command.PatientId}"));

        if(Errors.Count>0)
            return DTOs.Result<RegisterResponse>.Fail(Errors);

        return await _handler.Handle(command);

    }
}
