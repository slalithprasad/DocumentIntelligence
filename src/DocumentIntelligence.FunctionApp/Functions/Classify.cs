using DocumentIntelligence.Business.Interfaces;
using DocumentIntelligence.Business.Models.Request;
using DocumentIntelligence.Business.Models.Response;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DI.FunctionApp.Functions
{
    public class Classify
    {
        private readonly ILogger<Classify> _logger;
        private readonly IOpenAIService _openAIService;

        public Classify(ILogger<Classify> logger, IOpenAIService openAIService)
        {
            _logger = logger;
            _openAIService = openAIService;
        }

        [Function("classify")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            ApiResponse apiResponse = new ApiResponse();

            try
            {
                string? requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                if (string.IsNullOrEmpty(requestBody))
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.Error = "Request body is required.";
                    return new BadRequestObjectResult(apiResponse);
                }

                ClassifyRequest? request = JsonSerializer.Deserialize<ClassifyRequest>(requestBody);

                if (request is null)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.Error = "Request body must contain base64Content.";
                    return new BadRequestObjectResult(apiResponse);
                }

                InlineValidator<ClassifyRequest> validator = new InlineValidator<ClassifyRequest>
                {
                    ClassLevelCascadeMode = CascadeMode.Stop
                };

                validator.RuleFor(x => x.Base64Content)
                    .NotEmpty().WithMessage("Base64Content is required.")
                    .Matches(@"^data:([a-zA-Z0-9]+/[a-zA-Z0-9-.+]+)(;base64)?,.*$").WithMessage("Base64Content must be in data URI format.");

                var validation = await validator!.ValidateAsync(request!).ConfigureAwait(false);

                if (!validation.IsValid)
                {
                    apiResponse.IsSuccess = false;
                    apiResponse.Error = string.Join(", ", validation.Errors.Select(e => e.ErrorMessage));
                    return new BadRequestObjectResult(apiResponse);
                }

                ClassificationResponse classificationResponse = await _openAIService.ClassifyAsync(request).ConfigureAwait(false);

                apiResponse.IsSuccess = true;
                apiResponse.Result = classificationResponse;

                return new OkObjectResult(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{message}", ex.Message);

                apiResponse.IsSuccess = false;
                apiResponse.Error = "Something went wrong, please try again.";
                return new ObjectResult(apiResponse) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
