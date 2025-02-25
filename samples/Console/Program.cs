using System.Text.Json;
using DocumentIntelligence.Core.Extensions;
using DocumentIntelligence.Core.Interfaces;
using DocumentIntelligence.Core.Models.Configuration;
using DocumentIntelligence.Core.Models.Request;
using Microsoft.Extensions.DependencyInjection;
using TextCopy;

OpenAIConfiguration configuration = new OpenAIConfiguration
{
    Model = Environment.GetEnvironmentVariable("DI_Model"),
    Uri = Environment.GetEnvironmentVariable("DI_Uri"),
    Key = Environment.GetEnvironmentVariable("DI_Key")
};

Console.Write("please enter your filepath: ");
string? filePath = Console.ReadLine();

if (string.IsNullOrEmpty(filePath))
{
    Console.WriteLine("file path is required");
    return;
}

if (!string.IsNullOrEmpty(filePath) && filePath.StartsWith("\"") && filePath.EndsWith("\""))
{
    filePath = filePath.Substring(1, filePath.Length - 2);
}

if (!File.Exists(filePath))
{
    Console.WriteLine("file does not exist");
    return;
}

Console.Write("please enter the language: ");
string? language = Console.ReadLine();

if (string.IsNullOrEmpty(language))
{
    language = "english";
}

Console.Write("copy output to clipboard (y/n):");

string? copyToClipboard = Console.ReadLine();

if (string.IsNullOrEmpty(copyToClipboard))
{
    copyToClipboard = "n";
}

var services = new ServiceCollection();
services.AddServices(configuration);

using var serviceProvider = services.BuildServiceProvider();

var openAiService = serviceProvider.GetRequiredService<IOpenAIService>();

ExtractionRequest request = new ExtractionRequest
{
    DocumentName = Path.GetFileName(filePath),
    DocumentBase64 = Convert.ToBase64String(File.ReadAllBytes(filePath)),
    Language = language
};

Console.WriteLine("\n\nextracting...\n\n");

var response = await openAiService.ExtractAsync(request).ConfigureAwait(false);

Console.WriteLine(JsonSerializer.Serialize(response, new JsonSerializerOptions
{
    WriteIndented = true,
    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
}));

if (copyToClipboard.Equals("y", StringComparison.OrdinalIgnoreCase))
{
    ClipboardService.SetText(JsonSerializer.Serialize(response, new JsonSerializerOptions
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    }));
}