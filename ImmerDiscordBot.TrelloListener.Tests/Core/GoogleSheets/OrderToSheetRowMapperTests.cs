using FluentAssertions;
using ImmerDiscordBot.TrelloListener.Core.Shopify;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace ImmerDiscordBot.TrelloListener.Core.GoogleSheets
{
    public class OrderToSheetRowMapperTests
    {
        private OrderToSheetRowMapper _iut;

        [SetUp]
        protected void Setup()
        {
            _iut = new OrderToSheetRowMapper();
        }

        [TestCaseSource(typeof(OrderToSheetRowMapperDataSource))]
        public void Tests(string fileRelativePath, SheetRow expected)
        {
            var actual = GetOrderFromDataFile(fileRelativePath, NullLogger.Instance);

            actual.Should().BeEquivalentTo(expected);
        }

        private SheetRow GetOrderFromDataFile(string fileRelativePath, ILogger logger)
        {
            var message = FakeMessageBus.CreateRequest(fileRelativePath);
            var order = message.ToOrderObject(logger);
            var filter = new OrderCreatedFilter();
            var isBuild = filter.IsOrderForDactylKeyboard(order);
            if(!isBuild) Assert.Inconclusive($"Data file '{fileRelativePath}' is not a dactyl build");

            return _iut.MapToSheetRow(order);
        }
    }
}
