﻿using FluentAssertions;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;
using Microsoft.Extensions.Logging.Abstractions;
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

        [TestCaseSource(typeof(OrderToTrelloCardMapperDataSource))]
        public void Tests(string fileRelativePath, TrelloCardToCreate expected)
        {
            var actual = GetOrderFromDataFile(fileRelativePath);

            actual.Should().BeEquivalentTo(expected);
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
            Assert.That(actual.Notes, Is.EqualTo(null), "Notes on order failed expectations");
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
            Assert.That(actual.Notes, Is.EqualTo(null), "Notes on order failed expectations");
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
            Assert.That(actual.Notes, Is.EqualTo(null), "Notes on order failed expectations");
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
            Assert.That(actual.Notes, Is.EqualTo(null), "Notes on order failed expectations");
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
            Assert.That(actual.Notes, Is.EqualTo(null), "Notes on order failed expectations");
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
            Assert.That(actual.Notes, Is.EqualTo(null), "Notes on order failed expectations");
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
            var order = message.ToOrderObject(NullLogger.Instance);
            var filter = new OrderCreatedFilter();
            var isBuild = filter.IsOrderForDactylKeyboard(order);
            if(!isBuild) Assert.Inconclusive($"Data file '{fileRelativePath}' is not a dactyl build");

            return _iut.MapToTrelloCard(order);
        }
    }
}
