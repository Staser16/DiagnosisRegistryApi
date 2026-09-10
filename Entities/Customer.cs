namespace DiagnosisRepositoryApi.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string Pesel { get; set; } = string.Empty;
    public List<Record> Diagnoses { get; set; } = new();
}
