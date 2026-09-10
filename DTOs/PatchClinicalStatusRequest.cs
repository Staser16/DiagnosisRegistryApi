using System;
using DiagnosisRepositoryApi.Entities;

namespace DiagnosisRepositoryApi.DTOs;

public class PatchClinicalStatusRequest
{
    public ClinicalStatus ClinicalStatus { get; set; }
    public Provider Provider { get; set; }
    public Guid SourceId { get; set; }
}
