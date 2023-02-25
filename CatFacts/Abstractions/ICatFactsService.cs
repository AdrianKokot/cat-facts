using CatFacts.Entities;

namespace CatFacts.Abstractions;

public interface ICatFactsService
{
    public Task<CatFact> GetRandomFact(CancellationToken cancellationToken = default);
}