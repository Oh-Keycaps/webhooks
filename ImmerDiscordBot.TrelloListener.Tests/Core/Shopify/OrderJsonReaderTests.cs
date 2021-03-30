using System.IO;
using System.Linq;
using Microsoft.Azure.ServiceBus;
using NUnit.Framework;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    /// <summary>
    /// In these tests I only want to make sure that I'm reading minimal things in because the order object is HUGE.
    /// As of today I know i need name, and line_items. So lets test that.
    /// </summary>
    [TestFixture("data/order-3468.json")]
    [TestFixture("data/order-3472.json")]
    public class OrderJsonReaderTests
    {
        private Message _message;
        private OrderJsonReader _iut;

        public OrderJsonReaderTests(string fileRelativePath)
        {
            _message = FakeMessageBus.CreateMessage(fileRelativePath);
            _iut = new OrderJsonReader();
        }

        [Test]
        public void CanReadImportantProperties()
        {
            var order = _iut.ReadFromMessage(_message);

            Assert.That(order, Is.Not.Null);
            Assert.That(order.Name, Is.Not.Null);
            Assert.That(order.LineItems, Is.Not.Null & Is.Not.Empty);
            Assert.That(order.LineItems.All(x => x.ProductId.HasValue), "not all ProductIds have a value.. this isn't good");
            Assert.That(order.LineItems.All(x => x.VariantId.HasValue), "not all VariantIds have a value.. this isn't good");
        }

    }
}
