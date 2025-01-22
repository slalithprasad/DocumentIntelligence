using System.Text.Json.Serialization;

namespace DocumentIntelligence.Business.Models.Response
{
    public record ClassificationResponse(
        [property: JsonPropertyName("documentType")] string? DocumentType,
        [property: JsonPropertyName("issuer")] string? Issuer
        );
}