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
                OrderName = "#3773",
                Switches = null,
                MCU = "Two Elite Cs",
                CaseColor = "Black",
                CaseVariant = "Manuform 5x6",
                WristRestColor = "Navy",
                LEDs = null,
                IsDomestic = true,
                IsBluetooth = false,
                Notes = null,
                Accessories = new[]
                {
                    "TRRS Cables - Blue - 1.5m",
                    "Bottom Plate - 2x Bottom Plates",
                    "Wrist Rest Attachment",
                },
                ShopifyOrderUrl = "https://mechcaps.myshopify.com/admin/orders/3897056329839",
            });

            yield return new TrelloCardToCreateTestCaseData(new TrelloCardToCreate
            {
                CaseType = CaseTypes.DIY,
                OrderName = "#3855",
                MCU = "Two Elite Cs",
                CaseColor = "White",
                CaseVariant = "Manuform 5x6",
                WristRestColor = "Purple Gel",
                IsDomestic = true,
                Accessories = new[]
                {
                    "TRRS Cables - White/Black - 1.5m",
                    "Keycaps - DSA Pink/Purple Kits - Pink Scoops",
                    "Keycaps - DSA Pink/Purple Kits - 5x6 Manuform",
                    "Bottom Plate - 2x Bottom Plates",
                    "Wrist Rest Attachment",
                },
                ShopifyOrderUrl = "https://mechcaps.myshopify.com/admin/orders/3927426170991",
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
                Accessories = new[]
                {
                    "TRRS Cables - Blue - 1.5m",
                    "Keycaps - SA Pink/Blue",
                },
                ShopifyOrderUrl = "https://mechcaps.myshopify.com/admin/orders/4201889071215"
            });

            yield return new TrelloCardToCreateTestCaseData(new TrelloCardToCreate
            {
                CaseType = CaseTypes.DIY,
                OrderName = "#3993",
                Switches = null,
                MCU = "Two Elite Cs",
                CaseColor = "Wood",
                CaseVariant = "Dactyl",
                WristRestColor = "White",
                LEDs = null,
                IsDomestic = false,
                IsBluetooth = false,
                Notes = null,
                Accessories = new[]
                {
                    "TRRS Cables - White/Black - 1.5m",
                    "Wrist Rest Attachment",
                },
                ShopifyOrderUrl = "https://mechcaps.myshopify.com/admin/orders/4158907613295"
            });

            yield return new TrelloCardToCreateTestCaseData(new TrelloCardToCreate
            {
                CaseType = CaseTypes.DIY,
                OrderName = "#3822",
                Switches = null,
                MCU = "Two Elite Cs",
                CaseColor = "Gray",
                CaseVariant = "Manuform 5x6",
                WristRestColor = null,
                LEDs = null,
                IsDomestic = true,
                IsBluetooth = false,
                Notes = null,
                Accessories = new[]
                {
                    "TRRS Cables - Yellow - 1.5m",
                    "Bottom Plate - 2x Bottom Plates",
                    "No Wrist Rest",
                },
                ShopifyOrderUrl = "https://mechcaps.myshopify.com/admin/orders/3913295331439"
            });

            yield return new TrelloCardToCreateTestCaseData(new TrelloCardToCreate
            {
                CaseType = CaseTypes.DIY,
                OrderName = "#3827",
                Switches = null,
                MCU = "Two Elite Cs",
                CaseColor = "Copper",
                CaseVariant = "Manuform 5x6",
                WristRestColor = "Azure Blue",
                LEDs = null,
                IsDomestic = false,
                IsBluetooth = false,
                Notes = null,
                Accessories = new[]
                {
                    "TRRS Cables - White/Black - 1.5m",
                    "Bottom Plate - 2x Bottom Plates",
                    "Wrist Rest Attachment",
                },
                ShopifyOrderUrl = "https://mechcaps.myshopify.com/admin/orders/3915062313071"
            });

            yield return new TrelloCardToCreateTestCaseData(new TrelloCardToCreate
            {
                CaseType = CaseTypes.PETG_PLA,
                OrderName = "#3854",
                Switches = "Lubed Tealios",
                MCU = "Elite C",
                CaseColor = "Silk Blue",
                CaseVariant = "Dactyl",
                WristRestColor = "Azure Blue",
                LEDs = null,
                IsDomestic = false,
                IsBluetooth = false,
                Notes = null,
                Accessories = new[]
                {
                    "USB-C cables - Arubian Sea",
                    "TRRS Cables - Blue - 1.5m",
                    "Keycaps - Dactyl - SA Blue/Blue",
                },
                ShopifyOrderUrl = "https://mechcaps.myshopify.com/admin/orders/3927413620847"
            });

            yield return new TrelloCardToCreateTestCaseData(new TrelloCardToCreate
            {
                CaseType = CaseTypes.PETG_PLA,
                OrderName = "#4236",
                Switches = "Kailh Box Jade",
                MCU = "Elite C",
                CaseColor = "White",
                CaseVariant = "Dactyl",
                WristRestColor = "Black",
                LEDs = null,
                IsDomestic = false,
                IsBluetooth = false,
                Notes = null,
                Accessories = new[]
                {
                    "USB-C Blue/Black coiled cable",
                    "TRRS Cables - Blue/Black",
                    "SA Blue/Black/White blank keycaps",
                    "Red Escape Key",
                    "Black Scooped home keys",
                },
                ShopifyOrderUrl = "https://mechcaps.myshopify.com/admin/orders/4319636422767"
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
