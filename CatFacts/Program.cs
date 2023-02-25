using CatFacts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();

services.AddLogging(builder => builder.AddConsole())
        .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug)
        .AddSingleton<Host>();

var host = services.BuildServiceProvider().GetService<Host>();
var cancellationTokenRegistration = new CancellationTokenRegistration();

if (host is not null)
{
    await host.Main(cancellationTokenRegistration.Token);
}