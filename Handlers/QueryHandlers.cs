using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using DiagnosisRepositoryApi.Data;
using DiagnosisRepositoryApi.DTOs;
using DiagnosisRepositoryApi.Entities;

namespace DiagnosisRepositoryApi.Handlers;

public class CollectionHandler : IQueryHandler<object, List<CollectionResponse>>
{
    public async Task<List<CollectionResponse>> Handle(object command)
    {
        using var Connection = new SqliteConnection("Data Source=Rejestracja.db");

        var sql = "Select Code, Count(*) AS Count From Diagnoses Group By Code Order By Count desc";

        return (await Connection.QueryAsync<CollectionResponse>(sql)).ToList();
    }
}

public class RecognitionReadHandler(MyDbContext context) : IQueryHandler<ReadRecognitionRequest, DTOs.Result<List<ReadRecognitionResponse>>>
{
    public const int Size = 5;
    public async Task<DTOs.Result<List<ReadRecognitionResponse>>> Handle(ReadRecognitionRequest command)
    {
        List<DTOs.Error> Errors = new();
        Customer? patient;
        if (!string.IsNullOrWhiteSpace(command.Pesel))
        {
            patient = context.Patients.FirstOrDefault(Patient => Patient.Pesel == command.Pesel);
            if(patient is null)
                Errors.Add(new DTOs.Error.NotFoundPeselError(command.Pesel));
        }
        else
        {
            patient = context.Patients.FirstOrDefault(Patient => Patient.Id == command.PatientId);
            if(patient is null)
                Errors.Add(new DTOs.Error.NotFoundIdError(command.PatientId));
        }

        if (patient is null)
            return DTOs.Result<List<ReadRecognitionResponse>>.Fail(Errors);
            List<ReadRecognitionResponse> reads = new();

        var result = await context.Diagnoses
        .Where(x => x.PatientId == patient.Id && x.ClinicalStatus == command.ClinicalStatus)
        .OrderByDescending(x => x.CreationDate)
        .Skip((command.Page - 1) * Size)
        .Take(Size)
        .Select(x => new ReadRecognitionResponse
        {
            Code = x.Code,
            Provider = x.Provider,
            SourceId = x.SourceId,
            CodingSystem = x.CodingSystem,
            Description = x.Description,
            CreationDate = x.CreationDate,
            ClinicalStatus = x.ClinicalStatus,
            ConfirmationStatus = x.ConfirmationStatus,
            StartDate = x.StartDate,
            Age = x.Age,
            VisitId = x.VisitId,
            ReportingStatus = x.ReportingStatus
        })
        .ToListAsync();

        return DTOs.Result<List<ReadRecognitionResponse>>.Ok(result);
    }
}