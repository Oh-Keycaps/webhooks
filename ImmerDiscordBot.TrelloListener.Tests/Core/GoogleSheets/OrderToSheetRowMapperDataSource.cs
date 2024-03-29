﻿using System.Collections;
using System.Collections.Generic;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;
using NUnit.Framework;

namespace ImmerDiscordBot.TrelloListener.Core.GoogleSheets
{
    public class OrderToSheetRowMapperDataSource : IEnumerable<TestCaseData>
    {
        public IEnumerator<TestCaseData> GetEnumerator()
        {
            yield return new SheetRowTestCaseData(new SheetRow
            {
                CaseType = CaseTypes.SLA.ToString(),
                OrderName = "2607",
                MCU = "Elite C",
                CaseColor = null,
                CaseVariant = "Manuform 5x6",
                WristRestsIncluded = null,
                Notes = "Using my own switches",
                ShopifyOrderUrl = "https://mechcaps.myshopify.com/admin/orders/2298988298351",
            });

            yield return new SheetRowTestCaseData(new SheetRow
            {
                CaseType = CaseTypes.PETG_PLA.ToString(),
                OrderName = "3307",
                MCU = "Elite C",
                CaseColor = "White",
                CaseVariant = "Manuform 6x6",
                WristRestsIncluded = null,
                Notes = null,
                ShopifyOrderUrl = "https://mechcaps.myshopify.com/admin/orders/2726571016303",
            });

            yield return new SheetRowTestCaseData(new SheetRow
            {
                CaseType = CaseTypes.DIY.ToString(),
                OrderName = "3716",
                MCU = null,
                CaseColor = "Black w/ Transparent bottom",
                CaseVariant = "Manuform 4x6",
                WristRestsIncluded = "Wrist Rest Attachment",
                Notes = null,
                ShopifyOrderUrl = "https://mechcaps.myshopify.com/admin/orders/3868970975343",
            });

            yield return new SheetRowTestCaseData(new SheetRow
            {
                CaseType = CaseTypes.DIY.ToString(),
                OrderName = "3822",
                MCU = "Two Elite Cs",
                CaseColor = "Gray",
                CaseVariant = "Manuform 5x6",
                WristRestsIncluded = "No Wrist Rest",
                Notes = string.Empty,
                ShopifyOrderUrl = "https://mechcaps.myshopify.com/admin/orders/3913295331439",
            });

            yield return new SheetRowTestCaseData(new SheetRow
            {
                CaseType = CaseTypes.DIY.ToString(),
                OrderName = "3855",
                MCU = "Two Elite Cs",
                CaseColor = "White",
                CaseVariant = "Manuform 5x6",
                WristRestsIncluded = "Wrist Rest Attachment",
                Notes = null,
                ShopifyOrderUrl = "https://mechcaps.myshopify.com/admin/orders/3927426170991"
            });
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
