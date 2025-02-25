using System;
using DocumentIntelligence.Core.Interfaces;
using DocumentIntelligence.Core.Models.Configuration;
using DocumentIntelligence.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentIntelligence.Core.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, OpenAIConfiguration configuration)
    {
        services.AddSingleton(configuration);

        services.AddSingleton<IOpenAIService, OpenAIService>(); 

        return services;
    }
}
