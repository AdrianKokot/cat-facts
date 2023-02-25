namespace CatFacts.Abstractions;

public interface IDatabaseWriter
{
    public Task Add<T>(T item, CancellationToken cancellationToken = default);
}