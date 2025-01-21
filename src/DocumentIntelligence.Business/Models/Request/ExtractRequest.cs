using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DocumentIntelligence.Business.Models.Request
{
    public record ExtractRequest(
        [property: JsonPropertyName("base64Content")] string? Base64Content,
        [property: JsonPropertyName("language")] string? Language 
    );
}
