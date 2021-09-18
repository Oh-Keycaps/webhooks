using NUnit.Framework;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    [TestFixture]
    public class OrderCreatedFilterTests
    {
        private OrderCreatedFilter _iut;

        [SetUp]
        protected void Setup()
        {
            _iut = new OrderCreatedFilter();
        }

        [TestCase("data/order-3468.json", true)]
        [TestCase("data/order-3472.json", false)]
        [TestCase("data/order-2607.json", true)]
        [TestCase("data/order-3716.json", true)]
        public void IsOrderForDactylKeyboard(string fileRelativePath, bool expected)
        {
            var message = FakeMessageBus.CreateRequest(fileRelativePath);
            var order = message.ToOrderObject();

            var actual = _iut.IsOrderForDactylKeyboard(order);

            Assert.That(actual, Is.EqualTo(expected), "IsOrderForDactylKeyboard result is different than expected");
        }
    }
}
