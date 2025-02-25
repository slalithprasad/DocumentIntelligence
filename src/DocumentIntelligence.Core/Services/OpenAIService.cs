using System;
using System.ClientModel;
using System.Text.Json;
using Azure.AI.OpenAI;
using DocumentIntelligence.Core.Interfaces;
using DocumentIntelligence.Core.Models.Configuration;
using DocumentIntelligence.Core.Models.Request;
using DocumentIntelligence.Core.Models.Response;
using OpenAI.Chat;

namespace DocumentIntelligence.Core.Services;

public class OpenAIService(OpenAIConfiguration configuration) : IOpenAIService
{
    private readonly ChatClient chatClient = new AzureOpenAIClient(new Uri(configuration.Uri!), new ApiKeyCredential(configuration.Key!)).GetChatClient(configuration.Model!);

    public async Task<ExtractionResponse> ExtractAsync(ExtractionRequest request)
    {
        string mimeType = GetMimeType(request.DocumentName!);
        string language = string.IsNullOrEmpty(request.Language) ? "english" : request.Language;

        string prompt = $@"
                Extract the text from the following Base64-encoded document in {language}.

                Return a JSON object with these fields:
                - documentType (accepted identifiers: 'aadhar', 'pan', 'passport', 'emirates id', 'driving license'. If the document type is not one of these, use an empty string (''))
                - fullName
                - firstName
                - middleName
                - lastName
                - dateOfBirth
                - address
                - documentNumber
                - fatherName
                - motherName
                - gender
                - nationality (Attempt to determine the nationality based on the provided document. If the nationality is not explicitly mentioned, infer it from the document type. For instance, an Aadhaar card is issued exclusively to Indian citizens, so the nationality in that case would be 'Indian'.)
                - issuer
                - issueDate
                
                all the values extracted should be in {language} strictly except for dates, documentType and documentNumber.
            ";

        ChatMessageContentPart userChatTextContentPart = ChatMessageContentPart.CreateTextPart(prompt);

        BinaryData imageData = BinaryData.FromBytes(Convert.FromBase64String(request.DocumentBase64!));

        ChatMessageContentPart userImageContentPart = ChatMessageContentPart.CreateImagePart(imageData, mimeType);

        ChatMessage[] messages = new ChatMessage[]
        {
            new SystemChatMessage(@"You are an AI model designed to recognize document types and extract key-value pairs from images of documents. Output the results in a precise, minified JSON format without new lines or spaces. Format any dates as dd-MM-yyyy. Include all specified keys, and if a value is not found, use an empty string ('') instead of null. Follow these instructions strictly, and avoid adding extra text, markdown, formatting and backticks (`)."),

            new UserChatMessage(userChatTextContentPart, userImageContentPart)
        };

        ChatCompletion chatCompletion = await chatClient.CompleteChatAsync(messages, new ChatCompletionOptions
        {
            Temperature = 0.0f,
            TopP = 0.0f,
            MaxOutputTokenCount = 1000,
        }).ConfigureAwait(false);
        return JsonSerializer.Deserialize<ExtractionResponse>(chatCompletion!.Content.First().Text)!;
    }


    string GetMimeType(string documentName)
    {
        string extension = Path.GetExtension(documentName).ToLower();
        return extension switch
        {
            ".txt" => "text/plain",
            ".csv" => "text/csv",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".ppt" => "application/vnd.ms-powerpoint",
            ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            ".odt" => "application/vnd.oasis.opendocument.text",
            ".ods" => "application/vnd.oasis.opendocument.spreadsheet",
            ".odp" => "application/vnd.oasis.opendocument.presentation",
            ".rtf" => "application/rtf",
            ".pdf" => "application/pdf",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".tiff" or ".tif" => "image/tiff",
            ".svg" => "image/svg+xml",
            ".webp" => "image/webp",
            ".html" or ".htm" => "text/html",
            ".css" => "text/css",
            ".js" => "application/javascript",
            ".json" => "application/json",
            ".xml" => "application/xml",
            ".zip" => "application/zip",
            ".rar" => "application/vnd.rar",
            ".7z" => "application/x-7z-compressed",
            ".tar" => "application/x-tar",
            ".gz" => "application/gzip",
            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            ".ogg" => "audio/ogg",
            ".mp4" => "video/mp4",
            ".avi" => "video/x-msvideo",
            ".mov" => "video/quicktime",
            ".mkv" => "video/x-matroska",
            ".cs" => "text/plain",
            ".java" => "text/x-java-source",
            ".py" => "text/x-python",
            ".cpp" => "text/x-c++src",
            ".c" => "text/x-csrc",
            ".php" => "application/x-httpd-php",
            ".sh" => "application/x-sh",
            ".bat" => "application/x-msdos-program",
            _ => "application/octet-stream"
        };
    }
}




