using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.DiscordObjects;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;
using Moq.Protected;
using NUnit.Framework;

namespace ImmerDiscordBot.TrelloListener.Core.Discord
{
    [TestFixture]
    public class DiscordWebHookTests
    {
        private Mock<HttpMessageHandler> messageHandler;
        private DiscordWebHook iut;

        [SetUp]
        protected void Setup()
        {
            messageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var httpClient = new HttpClient(messageHandler.Object){BaseAddress = new Uri("https://discord.com")};
            var factoryMock = Mock.Of<IHttpClientFactory>(factory => factory.CreateClient(nameof(DiscordWebHook)) == httpClient);
            iut = new DiscordWebHook(Options.Create<DiscordSettings>(new DiscordSettings{WebhookId = "A", WebhookToken = "B"}), factoryMock);
        }

        [Test]
        public async Task DoIt()
        {
            messageHandler.SetupRequest(HttpMethod.Post, "https://discord.com/A/B")
                .Throws(new Exception("Hello World"));
            var content = await iut.ExecuteWebhook(new ExecuteWebhook());

            Assert.That(content, Is.EqualTo("Something"));
        }
    }

    internal static class MockHttpClientExtensions
    {
        public static ISetup<HttpMessageHandler, Task<HttpResponseMessage>> SetupRequest(this Mock<HttpMessageHandler> mock, HttpMethod method, string requestUri)
        {
            return mock.Protected().As<IMessageHandlerMock>()
                .Setup(x => x.SendAsync(IsMethodAndRequestUri(method, requestUri), It.IsAny<CancellationToken>()));
        }

        private static HttpRequestMessage IsMethodAndRequestUri(HttpMethod method, string requestUri)
        {
            return Match.Create<HttpRequestMessage>
            (
                message => message.Method == method && message.RequestUri == new Uri(requestUri),
                () => IsMethodAndRequestUri(method, requestUri)
            );
        }
    }

    public interface IMessageHandlerMock
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token);
    }
}
