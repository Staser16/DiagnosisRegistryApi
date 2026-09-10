using DiagnosisRepositoryApi.Entities;

namespace DiagnosisRepositoryApi.DTOs;

public class ReadRecognitionRequest
{
    public ClinicalStatus ClinicalStatus { get; set; }
    public string? Pesel { get; set; } = string.Empty;
    public Guid? PatientId { get; set; }
    public int Page {get;set;}
}