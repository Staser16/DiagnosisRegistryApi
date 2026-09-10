using System.ComponentModel.DataAnnotations;
using DiagnosisRepositoryApi.Entities;

namespace DiagnosisRepositoryApi.DTOs;

public class RejestrRequest
{
    public Provider Provider {get;set;}
    public Guid SourceId {get;set;}
    public string Code { get; set; } = string.Empty;
    [Range(1,3)]
    public int Attempt {get;set;}
}
