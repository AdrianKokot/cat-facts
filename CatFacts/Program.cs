using CatFacts;
using CatFacts.Abstractions;
using CatFacts.Database;
using CatFacts.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();

services.AddLogging(builder => builder.AddConsole())
        .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug)
        .AddHttpClient()
        .AddSingleton<IDatabaseWriter, TextFileDatabaseWriter>()
        .AddSingleton<ICatFactsService, CatFactsService>()
        .AddSingleton<Host>();

var host = services.BuildServiceProvider().GetService<Host>();
var cancellationTokenRegistration = new CancellationTokenRegistration();

await (host ?? throw new ArgumentNullException(nameof(Host))).Main(cancellationTokenRegistration.Token);