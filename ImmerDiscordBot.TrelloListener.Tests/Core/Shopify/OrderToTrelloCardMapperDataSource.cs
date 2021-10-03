using System.Collections;
using System.Collections.Generic;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;
using NUnit.Framework;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public class OrderToTrelloCardMapperDataSource : IEnumerable<TestCaseData>
    {
        public IEnumerator<TestCaseData> GetEnumerator()
        {
            yield return new TrelloCardToCreateTestCaseData(new TrelloCardToCreate
            {
                CaseType = CaseTypes.DIY,
                OrderName = "#3855",
                MCU = "Two Elite Cs",
                CaseColor = "White",
                CaseVariant = "Manuform 5x6",
                WristRestColor = "Purple Gel",
                IsDomestic = true,
                Accessories = new []
                {
                    "TRRS Cables - White/Black - 1.5m",
                    "Keycaps - DSA Pink/Purple Kits - Pink Scoops",
                    "Keycaps - DSA Pink/Purple Kits - 5x6 Manuform",
                    "Bottom Plate - 2x Bottom Plates",
                    "Wrist Rest Attachment",
                }
            });

            yield return new TrelloCardToCreateTestCaseData(new TrelloCardToCreate
            {
                CaseType = CaseTypes.PETG_PLA,
                OrderName = "#4080",
                Switches = "Lubed Tealios",
                MCU = "Elite C",
                CaseColor = "Silk Blue",
                CaseVariant = "Manuform 4x6",
                WristRestColor = "Azure Blue",
                LEDs = string.Empty,
                IsDomestic = true,
                IsBluetooth = true,
                Accessories = new []
                {
                    "TRRS Cables - Blue - 1.5m",
                    "Keycaps - SA Pink/Blue",
                }
            });
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class TrelloCardToCreateTestCaseData : TestCaseData
    {
        public TrelloCardToCreateTestCaseData(TrelloCardToCreate expected)
            : base($"data/order-{expected.OrderName.TrimStart('#')}.json", expected)
        {
            SetName($"Mapping Known Order {expected.OrderName}");
        }
    }
}
