using System;

namespace DocumentIntelligence.Core.Models.Configuration;

public class OpenAIConfiguration
{
    public string? Model { get; set; } = null!;
    public string? Uri { get; set; } = null!;
    public string? Key { get; set; } = null!;
}
