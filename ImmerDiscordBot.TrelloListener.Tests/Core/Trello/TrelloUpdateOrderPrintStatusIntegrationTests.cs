using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace ImmerDiscordBot.TrelloListener.Core.Trello
{
    [TestFixture, Explicit("these tests won't always work I just need to get this working")]
    public class TrelloUpdateOrderPrintStatusIntegrationTests
    {
        [Test]
        public async Task GetList()
        {
            const string boardId = "45yAlV3N";
            var provider = new TestFunctionsBuilder().Build();
            var service = provider.GetRequiredService<TrelloUpdateOrderPrintStatus>();

            await service.UpdateOrderPrintStatus(boardId, NullLogger.Instance);

            // Assert.That(trelloList, Is.Not.Null & Is.Not.Empty & Has.Count.EqualTo(10));
            // Assert.That(trelloList[0].Name, Is.EqualTo("Dactyls to Print"));
            // Assert.That(trelloList[0].Cards, Is.Not.Null & Is.Not.Empty);
            // Assert.That(trelloList[0].Cards[0].Name, Is.EqualTo("Order # 3483"));
            // Assert.That(trelloList[0].Cards[0].IsTemplate, Is.True);
        }
    }
}
