using DiagnosisRepositoryApi.Entities;

namespace DiagnosisRepositoryApi.DTOs;

public class Result<T>(T? value, List<Error> error) where T:class
{
    public bool IsGood => Error is null;
    public List<Error>? Error {get;set;} = error;
    public T? Value {get;set;} = value;
    public static Result<T> Ok(T value) => new Result<T>(value, null!);
    public static Result<T> Fail(List<Error> error) => new Result<T>(default, error);
}

public abstract record Error(string Code, string Description)
{
    public record ValidationError(string Problem) : Error("ValidationError", $"{Problem}");
    public record NotFoundIdError(Guid? Id) : Error("NotFoundError", $"Patient with following Id has not been found: {Id}");
    public record NotFoundPeselError(string? Pesel) : Error("NotFoundError", $"Patient with following Pesel has not been found: {Pesel}");
    public record NotFoundDiagnosisError(Guid Id, Provider System) : Error("NotFoundError", $"Diagnosis with Id: {Id} and system: {System} has not been found");
    public record DatabaseError(string Problem) : Error("DatabaseError", $"Database has occured a problem while executing commands: {Problem}");
}