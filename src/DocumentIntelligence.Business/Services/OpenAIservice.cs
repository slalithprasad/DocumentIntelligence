using Azure.AI.OpenAI;
using DocumentIntelligence.Business.Extensions;
using DocumentIntelligence.Business.Interfaces;
using DocumentIntelligence.Business.Models.Request;
using DocumentIntelligence.Business.Models.Response;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using System.ClientModel;
using System.Text.Json;

namespace DocumentIntelligence.Business.Services
{
    public class OpenAiservice : IOpenAIService
    {
        private readonly ChatClient client;

        public OpenAiservice(IConfiguration configuration)
        {
            string uri = configuration["OpenAIUri"]!;
            string key = configuration["OpenAIKey"]!;
            string model = configuration["OpenAIModel"]!;

            AzureOpenAIClient openAIClient = new(new Uri(uri), new ApiKeyCredential(key));
            client = openAIClient.GetChatClient(model);
        }

        public async Task<ClassificationResponse> ClassifyAsync(ClassifyRequest? request)
        {
            ArgumentNullException.ThrowIfNull(request);

            BinaryData imageBinaryData = BinaryData.FromBytes(Convert.FromBase64String(request.Base64Content!.GetCompatibleBase64String()));

            ChatMessageContentPart systemChatMessageTextContent = ChatMessageContentPart.CreateTextPart(@"You are an AI model designed to recognize document types and their issuing authority from images of documents. Recognize the document type and assign it to the 'documentType' key. Use the following identifiers for 'documentType': `Aadhar`, `Pan`, `DriverLicense`, `Voter`. Additionally, detect the issuing authority (which is the country of issuance) and assign it to the 'issuingAuthority' key as a string. If no document type or issuing authority is detected, set their respective values as empty strings. Output the results in a precise, minified JSON format, Do not use any formatting like Markdown etc and Follow these instructions strictly.");


            ChatMessageContentPart userImageContentPart = ChatMessageContentPart.CreateImagePart(imageBinaryData, request.Base64Content!.GetMimeTypeFromBase64());

            ChatMessage[] messages = new ChatMessage[]
            {
                 new SystemChatMessage(systemChatMessageTextContent),

                 new UserChatMessage(userImageContentPart)
            };

            ChatCompletion? completion = await client.CompleteChatAsync(messages).ConfigureAwait(false);

            ClassificationResponse response = JsonSerializer.Deserialize<ClassificationResponse>(completion!.Content.First().Text)!;

            return response;
        }

        public async Task<ExtractionResponse> ExtractAsync(ExtractRequest? request)
        {
            ArgumentNullException.ThrowIfNull(request);

            BinaryData imageBinaryData = BinaryData.FromBytes(Convert.FromBase64String(request.Base64Content!.GetCompatibleBase64String()));

            string extractLanguage = request.Language ?? "english";
            string translationText = extractLanguage.Equals("english") ? "" : $"If any values are identified in 'english', you need to translate them to {extractLanguage} including documentType.";

            ChatMessageContentPart systemChatMessageTextContent = ChatMessageContentPart.CreateTextPart(@$"You are an AI model designed to recognize document types and extract key information from images of documents. For each document, extract the following details: firstName, lastName, dateOfBirth (in dd-mm-YYYY format), address, documentNumber, fatherName, motherName, gender, nationality (look for fields such as 'Country', 'Nationality', or 'Issued By'), issuer, and issueDate (in dd-mm-YYYY format, look for 'issued' followed by date). Assign the detected values to the respective keys. Recognize the document type and assign it to the 'documentType' key. Use the following identifiers for 'documentType': `Aadhar`, `Pan`, `DriverLicense`, `Voter`. Extract all the values for the specified keys in {extractLanguage} language. {translationText}If any field is not detected, leave it as empty string. Output the results in a precise, minified JSON format. Do not use any formatting like Markdown etc and Follow these instructions strictly.");

            ChatMessageContentPart userImageContentPart = ChatMessageContentPart.CreateImagePart(imageBinaryData, request.Base64Content!.GetMimeTypeFromBase64());

            ChatMessage[] messages = new ChatMessage[]
            {
                 new SystemChatMessage(systemChatMessageTextContent),

                 new UserChatMessage(userImageContentPart)
            };

            ChatCompletion? completion = await client.CompleteChatAsync(messages).ConfigureAwait(false);

            ExtractionResponse response = JsonSerializer.Deserialize<ExtractionResponse>(completion!.Content.First().Text)!;

            return response;
        }
    }
}
