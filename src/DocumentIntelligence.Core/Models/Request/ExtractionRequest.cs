using System;

namespace DocumentIntelligence.Core.Models.Request;

public class ExtractionRequest
{
    public string? DocumentName { get; set; }
    public string? DocumentBase64 { get; set; }
    public string? Language { get; set; }
}
