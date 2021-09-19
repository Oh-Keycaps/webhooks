using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;
using NUnit.Framework;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    [TestFixture]
    public class OrderToTrelloCardMapperTests
    {
        private OrderToTrelloCardMapper _iut;

        [SetUp]
        protected void Setup()
        {
            _iut = new OrderToTrelloCardMapper();
        }

        [Test]
        public void MappingKnownOrder3854()
        {
            var actual = GetOrderFromDataFile("data/order-3854.json");

            Assert.That(actual.OrderName, Is.EqualTo("#3854"), "OrderName does not match expectations");
            Assert.That(actual.Switches, Is.EqualTo("Lubed Tealios"), "Switches does not match expectations");
            Assert.That(actual.MCU, Is.EqualTo("Elite C"), "MCU does not match expectations");
            Assert.That(actual.CaseColor, Is.EqualTo("Silk Blue"), "CaseColor does not match expectations");
            Assert.That(actual.CaseVariant, Is.EqualTo("Dactyl"), "CaseVariant does not match expectations");
            Assert.That(actual.WristRestColor, Is.EqualTo("Azure Blue"), "WristRestColor does not match expectations");
            Assert.That(actual.LEDs, Is.Null, "LEDs does not match expectations");
            Assert.That(actual.IsDomestic, Is.EqualTo(false), "IsDomestic does not match expectations");
            Assert.That(actual.IsBluetooth, Is.EqualTo(false), "IsBluetooth does not match expectations");
            Assert.That(actual.Accessories, Is.EqualTo(new []
            {
                "USB-C cables - Arubian Sea",
                "TRRS Cables - Blue - 1.5m",
                "Keycaps - Dactyl - SA Blue/Blue",
            }));
        }

        [Test]
        public void MappingKnownOrder3827()
        {
            var actual = GetOrderFromDataFile("data/order-3827.json");

            Assert.That(actual.OrderName, Is.EqualTo("#3827"), "OrderName does not match expectations");
            Assert.That(actual.Switches, Is.Null, "Switches does not match expectations");
            Assert.That(actual.MCU, Is.EqualTo("Two Elite Cs"), "MCU does not match expectations");
            Assert.That(actual.CaseColor, Is.EqualTo("Copper"), "CaseColor does not match expectations");
            Assert.That(actual.CaseVariant, Is.EqualTo("Manuform 5x6"), "CaseVariant does not match expectations");
            Assert.That(actual.WristRestColor, Is.EqualTo("Azure Blue"), "WristRestColor does not match expectations");
            Assert.That(actual.LEDs, Is.Null, "LEDs does not match expectations");
            Assert.That(actual.IsDomestic, Is.EqualTo(false), "IsDomestic does not match expectations");
            Assert.That(actual.IsBluetooth, Is.EqualTo(false), "IsBluetooth does not match expectations");
            Assert.That(actual.Accessories, Has.Length.EqualTo(3), "Accessories count does not match expectations");
            Assert.That(actual.Accessories, Is.EqualTo(new []
            {
                "TRRS Cables - White/Black - 1.5m",
                "Bottom Plate - 2x Bottom Plates",
                "Wrist Rest Attachment",
            }));
        }

        [Test]
        public void MappingKnownOrder3822()
        {
            var actual = GetOrderFromDataFile("data/order-3822.json");

            Assert.That(actual.OrderName, Is.EqualTo("#3822"), "OrderName does not match expectations");
            Assert.That(actual.Switches, Is.Null, "Switches does not match expectations");
            Assert.That(actual.MCU, Is.EqualTo("Two Elite Cs"), "MCU does not match expectations");
            Assert.That(actual.CaseColor, Is.EqualTo("Gray"), "CaseColor does not match expectations");
            Assert.That(actual.CaseVariant, Is.EqualTo("Manuform 5x6"), "CaseVariant does not match expectations");
            Assert.That(actual.WristRestColor, Is.EqualTo(null), "WristRestColor does not match expectations");
            Assert.That(actual.LEDs, Is.Null, "LEDs does not match expectations");
            Assert.That(actual.IsDomestic, Is.EqualTo(true), "IsDomestic does not match expectations");
            Assert.That(actual.IsBluetooth, Is.EqualTo(false), "IsBluetooth does not match expectations");
            Assert.That(actual.Accessories, Has.Length.EqualTo(3), "Accessories count does not match expectations");
            Assert.That(actual.Accessories, Is.EqualTo(new []
            {
                "TRRS Cables - Yellow - 1.5m",
                "Bottom Plate - 2x Bottom Plates",
                "No Wrist Rest",
            }));
        }

        [Test]
        public void MappingKnownOrder3773()
        {
            var actual = GetOrderFromDataFile("data/order-3773.json");

            Assert.That(actual.OrderName, Is.EqualTo("#3773"), "OrderName does not match expectations");
            Assert.That(actual.Switches, Is.Null, "Switches does not match expectations");
            Assert.That(actual.MCU, Is.EqualTo("Two Elite Cs"), "MCU does not match expectations");
            Assert.That(actual.CaseColor, Is.EqualTo("Black"), "CaseColor does not match expectations");
            Assert.That(actual.CaseVariant, Is.EqualTo("Manuform 5x6"), "CaseVariant does not match expectations");
            Assert.That(actual.WristRestColor, Is.EqualTo("Navy"), "WristRestColor does not match expectations");
            Assert.That(actual.LEDs, Is.Null, "LEDs does not match expectations");
            Assert.That(actual.IsDomestic, Is.EqualTo(true), "IsDomestic does not match expectations");
            Assert.That(actual.IsBluetooth, Is.EqualTo(false), "IsBluetooth does not match expectations");
            Assert.That(actual.Accessories, Has.Length.EqualTo(3), "Accessories count does not match expectations");
            Assert.That(actual.Accessories, Is.EqualTo(new []
            {
                "TRRS Cables - Blue - 1.5m",
                "Bottom Plate - 2x Bottom Plates",
                "Wrist Rest Attachment",
            }));
        }

        [Test]
        public void MappingKnownOrder3716()
        {
            var actual = GetOrderFromDataFile("data/order-3716.json");

            Assert.That(actual.OrderName, Is.EqualTo("#3716"), "OrderName does not match expectations");
            Assert.That(actual.Switches, Is.Null, "Switches does not match expectations");
            Assert.That(actual.MCU, Is.Null, "MCU does not match expectations");
            Assert.That(actual.CaseColor, Is.EqualTo("Black w/ Transparent bottom"), "CaseColor does not match expectations");
            Assert.That(actual.CaseVariant, Is.EqualTo("Manuform 4x6"), "CaseVariant does not match expectations");
            Assert.That(actual.WristRestColor, Is.Null, "WristRestColor does not match expectations");
            Assert.That(actual.LEDs, Is.Null, "LEDs does not match expectations");
            Assert.That(actual.IsDomestic, Is.EqualTo(true), "IsDomestic does not match expectations");
            Assert.That(actual.IsBluetooth, Is.EqualTo(false), "IsBluetooth does not match expectations");
            Assert.That(actual.Accessories, Has.Length.EqualTo(1), "Accessories does not match expectations");
            Assert.That(actual.Accessories, Is.EqualTo(new []
            {
                "Wrist Rest Attachment",
            }), "Accessories does not match expectations");
        }

        [Test]
        public void MappingKnownOrder3468()
        {
            var actual = GetOrderFromDataFile("data/order-3468.json");

            Assert.That(actual.OrderName, Is.EqualTo("#3468"));
            Assert.That(actual.Switches, Is.EqualTo("Cherry MX Brown"));
            Assert.That(actual.MCU, Is.EqualTo("Elite C"));
            Assert.That(actual.CaseColor, Is.EqualTo("White"));
            Assert.That(actual.CaseVariant, Is.EqualTo("Manuform 6x6"));
            Assert.That(actual.WristRestColor, Is.EqualTo("Azure Blue"));
            Assert.That(actual.LEDs, Is.EqualTo(""));
            Assert.That(actual.IsDomestic, Is.EqualTo(true));
            Assert.That(actual.IsBluetooth, Is.EqualTo(false));
            Assert.That(actual.Accessories, Is.EqualTo(new []
            {
                "USB-C cables - White - Long (Dec 1st pre-order)",
                "TRRS Cables - Blue - 1.5m"
            }), "Accessories does not match expectations");
        }

        [Test]
        public void MappingKnownOrder3499()
        {
            var actual = GetOrderFromDataFile("data/order-3499.json");

            Assert.That(actual.OrderName, Is.EqualTo("#3499"));
            Assert.That(actual.Switches, Is.EqualTo("Cherry MX Blue"));
            Assert.That(actual.MCU, Is.EqualTo("Elite C"));
            Assert.That(actual.CaseColor, Is.EqualTo("Black w/ Transparent bottom"));
            Assert.That(actual.CaseVariant, Is.EqualTo("Manuform 5x6"));
            Assert.That(actual.WristRestColor, Is.EqualTo("Black"));
            Assert.That(actual.LEDs, Is.EqualTo("3x Strips of 4x LEDs per Side"));
            Assert.That(actual.IsDomestic, Is.EqualTo(true));
            Assert.That(actual.IsBluetooth, Is.EqualTo(false));
            Assert.That(actual.Accessories, Is.EqualTo(new []
            {
                "TRRS Cables - White/Black - 1.5m",
            }), "Accessories does not match expectations");
        }

        [Test]
        public void MappingKnownOrder3508()
        {
            var actual = GetOrderFromDataFile("data/order-3508.json");

            Assert.That(actual.OrderName, Is.EqualTo("#3508"));
            Assert.That(actual.Switches, Is.EqualTo("Cherry MX Blue"));
            Assert.That(actual.MCU, Is.EqualTo("Elite C"));
            Assert.That(actual.CaseColor, Is.EqualTo("Silk Blue"));
            Assert.That(actual.CaseVariant, Is.EqualTo("Manuform 6x6"));
            Assert.That(actual.WristRestColor, Is.EqualTo("Pink Gel"));
            Assert.That(actual.LEDs, Is.EqualTo("3x Strips of 4x LEDs per Side"));
            Assert.That(actual.IsDomestic, Is.EqualTo(false));
            Assert.That(actual.IsBluetooth, Is.EqualTo(false));
            Assert.That(actual.Accessories, Is.EqualTo(new []
            {
                "USB-C cables - White - Long (Dec 1st pre-order)",
                "TRRS Cables - White/Black - 1.5m",
                "Keycaps - DSA Pink/Purple",
            }), "Accessories does not match expectations");
        }

        [Test]
        public void MappingPaintJobOrder2607()
        {
            var actual = GetOrderFromDataFile("data/order-2607.json");

            Assert.That(actual.OrderName, Is.EqualTo("#2607"));
            Assert.That(actual.Switches, Is.EqualTo(string.Empty));
            Assert.That(actual.MCU, Is.EqualTo("Elite C"));
            Assert.That(actual.CaseColor, Is.Null);
            Assert.That(actual.CaseVariant, Is.EqualTo("Manuform 5x6"));
            Assert.That(actual.WristRestColor, Is.EqualTo("Black"));
            Assert.That(actual.LEDs, Is.EqualTo(string.Empty));
            Assert.That(actual.IsDomestic, Is.EqualTo(false));
            Assert.That(actual.PaintCaseColor, Is.EqualTo("Black"));
            Assert.That(actual.IsBluetooth, Is.EqualTo(false));
            Assert.That(actual.Accessories, Is.Empty);
        }

        [Test]
        public void MappingBluetooth3307()
        {
            var actual = GetOrderFromDataFile("data/order-3307.json");

            Assert.That(actual.OrderName, Is.EqualTo("#3307"));
            Assert.That(actual.Switches, Is.EqualTo("Lubed Healios"));
            Assert.That(actual.MCU, Is.EqualTo("Elite C"));
            Assert.That(actual.CaseColor, Is.EqualTo("White"));
            Assert.That(actual.CaseVariant, Is.EqualTo("Manuform 6x6"));
            Assert.That(actual.WristRestColor, Is.EqualTo("Black"));
            Assert.That(actual.LEDs, Is.EqualTo("3x Strips of 4x LEDs per Side"));
            Assert.That(actual.IsDomestic, Is.EqualTo(true));
            Assert.That(actual.IsBluetooth, Is.EqualTo(true));
            Assert.That(actual.Accessories, Is.EqualTo(new []
            {
                "USB-C cables - Red w/ Black Techflex - Long (Dec 1st pre-order)",
                "TRRS Cables - Green - 1.5m",
                "Keycaps - SA White/Black",
            }), "Accessories does not match expectations");
        }

        private TrelloCardToCreate GetOrderFromDataFile(string fileRelativePath)
        {
            var message = FakeMessageBus.CreateRequest(fileRelativePath);
            var order = message.ToOrderObject();
            var filter = new OrderCreatedFilter();
            var isBuild = filter.IsOrderForDactylKeyboard(order);
            if(!isBuild) Assert.Inconclusive($"Data file '{fileRelativePath}' is not a dactyl build");

            return _iut.MapToTrelloCard(order);
        }
    }
}
