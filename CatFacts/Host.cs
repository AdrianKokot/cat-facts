using CatFacts.Abstractions;
using CatFacts.Exceptions;
using Microsoft.Extensions.Logging;

namespace CatFacts;

public sealed class Host
{
    private readonly ICatFactsService _catFactsService;
    private readonly IDatabaseWriter _databaseWriter;
    private readonly ILogger<Host> _logger;

    public Host(ICatFactsService catFactsService, IDatabaseWriter databaseWriter, ILogger<Host> logger)
    {
        _catFactsService = catFactsService;
        _databaseWriter = databaseWriter;
        _logger = logger;
    }

    public async Task Main(CancellationToken cancellationToken = default)
    {
        try
        {
            var fact = await _catFactsService.GetRandomFact(cancellationToken);

            await _databaseWriter.Add(fact, cancellationToken);
        }
        catch (CatFactRequestFailedException exception)
        {
            _logger.LogWarning("{Message}", exception.Message);
        }
        catch (ArgumentNullException exception)
        {
            _logger.LogWarning("{Message}", exception.Message);
        }
    }
}