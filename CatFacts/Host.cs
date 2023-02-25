namespace CatFacts;

public sealed class Host
{
    public Task Main(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}