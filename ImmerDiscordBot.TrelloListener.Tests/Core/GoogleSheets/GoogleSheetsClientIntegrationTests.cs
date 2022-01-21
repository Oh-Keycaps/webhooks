using System.Collections.Generic;
using System.Linq;
using ImmerDiscordBot.TrelloListener.Contracts.GoogleSheets.Models;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ImmerDiscordBot.TrelloListener.Core.GoogleSheets
{
    [TestFixture, Explicit("This test calls google sheets and should only be run when I say to run it")]
    public class GoogleSheetsClientIntegrationTests
    {
        [Test]
        public void GetsPETGSheet()
        {
            var provider = new TestFunctionsBuilder().Build();
            var sheetsServiceProvider = provider.GetRequiredService<OrderPrintStatusProvider>();

            var petgAdapters = sheetsServiceProvider.GetOrderPrintStatuses();

            AssertOrderStatus(petgAdapters, orderNumber: "1885", expectedIsKeyboardPrinting: true,expectedIsShipped: true);
            AssertOrderStatus(petgAdapters, orderNumber: "4126", expectedIsKeyboardPrinting: true,expectedIsShipped: true);
            AssertOrderStatus(petgAdapters, orderNumber: "4161", expectedIsKeyboardPrinting: true,expectedIsShipped: false);
            AssertOrderStatus(petgAdapters, orderNumber: "4262", expectedIsKeyboardPrinting: false,expectedIsShipped: false);
            AssertOrderStatus(petgAdapters, orderNumber: "4000", expectedIsKeyboardPrinting: false,expectedIsShipped: false);
            AssertOrderStatus(petgAdapters, orderNumber: "3562", expectedIsKeyboardPrinting: true,expectedIsShipped: true);
            AssertOrderStatus(petgAdapters, orderNumber: "3723", expectedIsKeyboardPrinting: false,expectedIsShipped: false);
        }

        private void AssertOrderStatus(IEnumerable<OrderPrintStatus> orders, string orderNumber, bool expectedIsKeyboardPrinting, bool expectedIsShipped)
        {
            var foundOrder = orders.SingleOrDefault(x => x.Order == orderNumber);
            Assert.That(foundOrder, Is.Not.Null, "Could not find {0} in the list of orders", orderNumber);
            Assert.That(foundOrder.IsShipped, Is.EqualTo(expectedIsShipped), "The status IsShipped is different than expected for order {0}", orderNumber);
            Assert.That(foundOrder.IsKeyboardPrinting, Is.EqualTo(expectedIsKeyboardPrinting), "the status IsKeyboardPrinting is different than expected for order {0}", orderNumber);
        }
    }
}
