using System;
using DocumentIntelligence.Core.Models;
using DocumentIntelligence.Core.Models.Request;
using DocumentIntelligence.Core.Models.Response;

namespace DocumentIntelligence.Core.Interfaces;

public interface IOpenAIService
{
    Task<ExtractionResponse> ExtractAsync(ExtractionRequest request);
}
