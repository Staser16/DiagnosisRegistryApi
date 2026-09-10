using System.ComponentModel.DataAnnotations;
using DiagnosisRepositoryApi.Entities;
namespace DiagnosisRepositoryApi.DTOs;

public class RegisterRequest
{
    [Required]
    [Range(0, 1)]
    public Provider Provider { get; set; } = default!;
    public Guid SourceId {get;set;}
    public string Code { get; set; } = string.Empty;
    public string CodingSystem {get;set;} = string.Empty;
    public string Description {get;set;}= string.Empty;
    public DateOnly CreationDate { get; set; }
    [Required]
    [Range(0, 2)]
    public ClinicalStatus ClinicalStatus { get; set; } = default!;
    [Required]
    [Range(0, 1)]
    public ConfirmationStatus ConfirmationStatus { get; set; } = default!;
    public DateOnly? StartDate { get; set; }
    public int? Age { get; set; }
    public Guid PatientId { get; set; }
    public Guid? VisitId {get;set;}
}
