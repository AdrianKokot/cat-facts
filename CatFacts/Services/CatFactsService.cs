using System.Net.Http.Json;
using System.Text.Json;
using CatFacts.Abstractions;
using CatFacts.Entities;
using CatFacts.Exceptions;
using Microsoft.Extensions.Logging;

namespace CatFacts.Services;

public sealed class CatFactsService : ICatFactsService
{
    private const string GetFactRequestUri = "fact";
    private static readonly Uri BaseAddress = new("https://catfact.ninja");
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<CatFactsService> _logger;

    public CatFactsService(ILogger<CatFactsService> logger, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _clientFactory = clientFactory;
    }

    public async Task<CatFact> GetRandomFact(CancellationToken cancellationToken = default)
    {
        using var client = _clientFactory.CreateClient(nameof(CatFactsService));
        client.BaseAddress = BaseAddress;

        _logger.LogDebug("Sending request to {Address}{Path}",
            client.BaseAddress, GetFactRequestUri);

        var response = await client.GetAsync(GetFactRequestUri, cancellationToken);

        _logger.LogDebug("Got response with code \"{Code}\"",
            response.StatusCode);

        if (!response.IsSuccessStatusCode)
        {
            var responseError = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);

            throw new CatFactRequestFailedException(responseError);
        }

        CatFact? fact;
        
        try
        {
            fact = await response.Content.ReadFromJsonAsync<CatFact>(cancellationToken: cancellationToken);
        }
        catch (JsonException e)
        {
            throw new CatFactRequestFailedException(e.Message);
        }

        if (fact is null) throw new CatFactRequestFailedException("Response was null");

        return fact;
    }
}