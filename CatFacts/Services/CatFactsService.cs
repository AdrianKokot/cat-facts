using System.Net.Http.Json;
using CatFacts.Abstractions;
using CatFacts.Entities;
using CatFacts.Exceptions;
using Microsoft.Extensions.Logging;

namespace CatFacts.Services;

public sealed class CatFactsService : ICatFactsService
{
    private const string GetFactRequestUri = "fact";
    private static readonly Uri BaseAddress = new("https://catfact.ninja");
    private readonly ILogger<CatFactsService> _logger;

    public CatFactsService(ILogger<CatFactsService> logger)
    {
        _logger = logger;
    }

    public async Task<CatFact> GetRandomFact(CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient
        {
            BaseAddress = BaseAddress
        };

        _logger.LogDebug("Sending request to {Address}{Path}",
            client.BaseAddress, GetFactRequestUri);

        var response = await client.GetAsync(GetFactRequestUri, cancellationToken);

        _logger.LogDebug("Got response with code \"{Code}\"",
            response.StatusCode);

        if (!response.IsSuccessStatusCode)
        {
            var responseError =
                (await response.Content.ReadFromJsonAsync<object>(cancellationToken: cancellationToken))
                ?.ToString() ?? response.StatusCode.ToString();

            throw new CatFactRequestFailedException(responseError);
        }

        var fact = await response.Content.ReadFromJsonAsync<CatFact>(cancellationToken: cancellationToken);

        if (fact is null) throw new CatFactRequestFailedException("Response was null.");

        return fact;
    }
}