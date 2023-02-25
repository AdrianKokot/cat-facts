using System.Net;
using System.Text.Json;
using CatFacts.Entities;
using CatFacts.Exceptions;
using CatFacts.Services;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;

namespace CatFacts.Test;

[TestFixture]
[TestOf(typeof(CatFactsService))]
public class CatFactsServiceTests
{
    [Test]
    public async Task GetRandomFact_should_get_fact_from_API()
    {
        var testCatFact = new CatFact("Baking chocolate is the most dangerous chocolate to your cat.", 61);

        var handler = new MockHttpMessageHandler();
        handler.When(HttpMethod.Get, new Uri("https://catfact.ninja/fact").ToString())
               .Respond(HttpStatusCode.OK, new StringContent(JsonSerializer.Serialize(testCatFact)));

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(handler.ToHttpClient());

        var mockLogger = new Mock<ILogger<CatFactsService>>();

        var service = new CatFactsService(mockLogger.Object, mockHttpClientFactory.Object);

        var fact = await service.GetRandomFact();

        Assert.That(fact, Is.EqualTo(testCatFact));
    }

    [Test]
    public void GetRandomFact_should_throw_CatFactRequestFailedException_on_failed_request()
    {
        var handler = new MockHttpMessageHandler();
        handler.When(HttpMethod.Get, new Uri("https://catfact.ninja/fact").ToString())
               .Respond(HttpStatusCode.Forbidden, new StringContent("{}"));
        
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(handler.ToHttpClient());

        var mockLogger = new Mock<ILogger<CatFactsService>>();

        var service = new CatFactsService(mockLogger.Object, mockHttpClientFactory.Object);
        
        Assert.ThrowsAsync<CatFactRequestFailedException>(async () => await service.GetRandomFact());
    }
    
    [Test]
    public void GetRandomFact_should_throw_CatFactRequestFailedException_on_empty_response()
    {
        var handler = new MockHttpMessageHandler();
        handler.When(HttpMethod.Get, new Uri("https://catfact.ninja/fact").ToString())
               .Respond(HttpStatusCode.Forbidden, new StringContent(""));
        
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(handler.ToHttpClient());

        var mockLogger = new Mock<ILogger<CatFactsService>>();

        var service = new CatFactsService(mockLogger.Object, mockHttpClientFactory.Object);
        
        Assert.ThrowsAsync<CatFactRequestFailedException>(async () => await service.GetRandomFact());
    }
}