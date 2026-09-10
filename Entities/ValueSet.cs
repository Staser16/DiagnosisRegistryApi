using System.Text.Json.Serialization;

namespace DiagnosisRepositoryApi.Entities;


[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(DiagnosesConcept), "DIAGNOSIS")]
public abstract record Concept;

public record DiagnosesConcept(
    string Code,
    string Display
) : Concept;

public record Include(
    string System,
    List<Concept> Concept
);

public record Compose(
    List<Include> Include
);

public record ValueSet(
    string ResourceType,
    string Id,
    string Url,
    string Version,
    string Name,
    string Title,
    string Status,
    string Description,
    Compose Compose
);