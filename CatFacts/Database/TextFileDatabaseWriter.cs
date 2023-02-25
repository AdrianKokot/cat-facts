using CatFacts.Abstractions;
using Microsoft.Extensions.Logging;

namespace CatFacts.Database;

public sealed class TextFileDatabaseWriter : IDatabaseWriter
{
    private const string FilePath = @".\database.txt";
    private readonly ILogger<TextFileDatabaseWriter> _logger;

    public TextFileDatabaseWriter(ILogger<TextFileDatabaseWriter> logger)
    {
        _logger = logger;
    }

    public async Task Add<T>(T item, CancellationToken cancellationToken = default)
    {
        var content = item?.ToString();

        if (content is null) throw new ArgumentNullException(nameof(item));

        await File.AppendAllTextAsync(FilePath, content + Environment.NewLine, cancellationToken);
        _logger.LogDebug("Saved to file: {Data}", content);
    }
}