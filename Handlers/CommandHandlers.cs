using System.Net;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.ObjectPool;
using DiagnosisRepositoryApi.Data;
using DiagnosisRepositoryApi.DTOs;
using DiagnosisRepositoryApi.Entities;
using DiagnosisRepositoryApi.Services;

namespace DiagnosisRepositoryApi.Handlers;

public class RegisterHandler(MyDbContext context, ValueSet valueSet, ICommandHandler<RejestrRequest, HttpResponseMessage> handler) : ICommandHandler<RegisterRequest, DTOs.Result<RegisterResponse>>
{
    public async Task<DTOs.Result<RegisterResponse>> Handle(RegisterRequest command)
    {
        var description = valueSet.Compose.Include
        .SelectMany(Include => Include.Concept)
        .OfType<DiagnosesConcept>()
        .Where(Concept => Concept.Code == command.Code)
        .Select(concept => concept.Display)
        .FirstOrDefault()!;

        var patient = await context.Patients.FindAsync(command.PatientId);

        if (context.Diagnoses.Any(x => x.SourceId == command.SourceId && x.Provider == command.Provider))
        {
            var Status = context.Diagnoses.Where(x=>x.Provider == command.Provider && x.SourceId == command.SourceId).Select(x=>x.ReportingStatus).FirstOrDefault();
            return MapResult(command, description, Status);
        }

        Record diagnosis = new Record
        {
            SourceId = command.SourceId,
            Provider = command.Provider,
            Code = command.Code,
            CodingSystem = command.CodingSystem,
            Description = description,
            CreationDate = command.CreationDate,
            ClinicalStatus = command.ClinicalStatus,
            ConfirmationStatus = command.ConfirmationStatus,
            StartDate = command.StartDate,
            Age = command.Age,
            PatientId = command.PatientId,
            Patient = patient!,
            VisitId = command.VisitId,
            ReportingStatus = ReportingStatus.Pending
        };

        await context.Diagnoses.AddAsync(diagnosis);
        await context.SaveChangesAsync();

        var response = await handler.Handle(new RejestrRequest
        {
            Code = diagnosis.Code,
            SourceId = diagnosis.SourceId,
            Provider = diagnosis.Provider
        });

        if (response.StatusCode == HttpStatusCode.Accepted)
        {
            diagnosis.ReportingStatus = ReportingStatus.Reported;
        }
        else
        {
            diagnosis.ReportingStatus = ReportingStatus.Failed;
        }

        await context.SaveChangesAsync();

        return MapResult(diagnosis);
    }
    private DTOs.Result<RegisterResponse> MapResult(Record diagnosis)
    {
        return DTOs.Result<RegisterResponse>.Ok(
        new RegisterResponse
        {
            SourceId = diagnosis.SourceId,
            Provider = diagnosis.Provider,
            ICD10 = diagnosis.Code,
            CodingSystem = diagnosis.CodingSystem,
            Description = diagnosis.Description,
            CreationDate = diagnosis.CreationDate,
            ClinicalStatus = diagnosis.ClinicalStatus,
            ConfirmationStatus = diagnosis.ConfirmationStatus,
            StartDate = diagnosis.StartDate,
            Age = diagnosis.Age,
            PatientId = diagnosis.PatientId,
            VisitId = diagnosis.VisitId,
            ReportingStatus = diagnosis.ReportingStatus
        });
    }
    private DTOs.Result<RegisterResponse> MapResult(RegisterRequest request, string description, ReportingStatus status)
    {
        return DTOs.Result<RegisterResponse>.Ok(
        new RegisterResponse
        {
            SourceId = request.SourceId,
            Provider = request.Provider,
            ICD10 = request.Code,
            CodingSystem = request.CodingSystem,
            Description = description,
            CreationDate = request.CreationDate,
            ClinicalStatus = request.ClinicalStatus,
            ConfirmationStatus = request.ConfirmationStatus,
            StartDate = request.StartDate,
            Age = request.Age,
            PatientId = request.PatientId,
            VisitId = request.VisitId,
            ReportingStatus = status
        });
    }
}

public class RejestrHandler(RetryService service, HttpClient httpClient) : ICommandHandler<RejestrRequest, HttpResponseMessage>
{
    public async Task<HttpResponseMessage> Handle(RejestrRequest command)
    {
         var response = await service.ExecuteAsync(
            async (attempt) =>
            {
                command.Attempt = attempt;

                return await httpClient.PostAsJsonAsync(
                    "api/record/Rejestr",
                    command);
            },
            response =>
                response.StatusCode == HttpStatusCode.ServiceUnavailable
        );

        return response;
    }
}

public class PatchClinicalStatusHandler : ICommandHandler<PatchClinicalStatusRequest, DTOs.Result<Object>>
{
    public async Task<DTOs.Result<object>> Handle(PatchClinicalStatusRequest command)
    {
        using var Connection = new SqliteConnection("Data Source=Rejestracja.db");

        const string sql = """
        Update Diagnoses 
        SET ClinicalStatus = @ClinicalStatus
        WHERE SourceId = @SourceId 
        AND Provider = @Provider 
        """;

        var AffectedRows = await Connection.ExecuteAsync(sql, new { ClinicalStatus = (int)command.ClinicalStatus, SourceId = command.SourceId.ToString(), Provider = (int)command.Provider });

        if (AffectedRows == 0)
        {
            List<DTOs.Error> Errors = new();
            Errors.Add(new DTOs.Error.DatabaseError("Updating was not successfull"));
            return DTOs.Result<Object>.Fail(Errors);
        }

            return DTOs.Result<Object>.Ok(new Object());
    }
}