using NUnit.Framework;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    [TestFixture]
    public class OrderToTrelloCardMapperTests
    {
        [Test]
        public void MappingKnownOrder()
        {
            var message = FakeMessageBus.CreateMessage("data/order-3468.json");
            var order = message.ToOrderObject();
            var iut = new OrderToTrelloCardMapper();

            var actual = iut.MapToTrelloCard(order);

            Assert.That(actual.OrderName, Is.EqualTo("#3468"));
            Assert.That(actual.Switches, Is.EqualTo("Cherry MX Brown"));
            Assert.That(actual.MCU, Is.EqualTo("Elite C"));
            Assert.That(actual.CaseColor, Is.EqualTo("White"));
            Assert.That(actual.CaseVariant, Is.EqualTo("Manuform 6x6"));
            Assert.That(actual.WristRestColor, Is.EqualTo("Azure Blue"));
            Assert.That(actual.LEDs, Is.EqualTo(""));
            Assert.That(actual.IsDomestic, Is.EqualTo(true));
            Assert.That(actual.IsBluetooth, Is.EqualTo(false));
            Assert.That(actual.Accessories[0], Is.EqualTo("USB-C cables - White - Long (Dec 1st pre-order)"));
            Assert.That(actual.Accessories[1], Is.EqualTo("TRRS Cables - Blue - 1.5m"));
        }

        [Test]
        public void MappingKnownOrder3499()
        {
            var message = FakeMessageBus.CreateMessage("data/order-3499.json");
            var order = message.ToOrderObject();
            var iut = new OrderToTrelloCardMapper();

            var actual = iut.MapToTrelloCard(order);

            Assert.That(actual.OrderName, Is.EqualTo("#3499"));
            Assert.That(actual.Switches, Is.EqualTo("Cherry MX Blue"));
            Assert.That(actual.MCU, Is.EqualTo("Elite C"));
            Assert.That(actual.CaseColor, Is.EqualTo("Black w/ Transparent bottom"));
            Assert.That(actual.CaseVariant, Is.EqualTo("Manuform 5x6"));
            Assert.That(actual.WristRestColor, Is.EqualTo("Black"));
            Assert.That(actual.LEDs, Is.EqualTo("3x Strips of 4x LEDs per Side"));
            Assert.That(actual.IsDomestic, Is.EqualTo(true));
            Assert.That(actual.IsBluetooth, Is.EqualTo(false));
            Assert.That(actual.Accessories[0], Is.EqualTo("TRRS Cables - White/Black - 1.5m"));
        }
    }
}
