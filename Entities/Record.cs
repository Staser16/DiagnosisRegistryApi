namespace DiagnosisRepositoryApi.Entities;

public class Record
{
    public Provider Provider { get; set; } = default!;
    public Guid SourceId {get;set;}
    public string Code { get; set; } = string.Empty;
    public string CodingSystem {get;set;} = string.Empty;
    public string Description {get;set;}= string.Empty;
    public DateOnly CreationDate { get; set; }
    public ClinicalStatus ClinicalStatus { get; set; } = default!;
    public ConfirmationStatus ConfirmationStatus { get; set; } = default!;
    public DateOnly? StartDate { get; set; }
    public int? Age { get; set; }
    public Customer Patient { get; set; } = default!;
    public Guid PatientId { get; set; }
    public Guid? VisitId { get; set; }
    public ReportingStatus ReportingStatus {get;set;}
}

public enum ClinicalStatus
{
    Active,
    Healed,
    Recurrence
}

public enum ConfirmationStatus
{
    Confirmed,
    Suspicion
}

public enum Provider
{
    SYS_A,
    SYS_B
}

public enum ReportingStatus
{
    Pending,
    Reported,
    Failed
}