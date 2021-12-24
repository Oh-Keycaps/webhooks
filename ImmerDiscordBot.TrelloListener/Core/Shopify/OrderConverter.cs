using System.Collections.Generic;
using System.Linq;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public class OrderConverter : IOrderConverter
    {
        public Order Convert(ShopifyObjects.Order order)
        {
            var convert = new Order
            {
                Id = order.Id.GetValueOrDefault(-1),
                Name = order.Name,
                ShippingAddressCountryCode = order.ShippingAddress.CountryCode,
                LineItems = Convert(order.LineItems),
                Notes = order.Note,
            };
            return convert;
        }

        private static LineItem[] Convert(IEnumerable<ShopifyObjects.LineItem> lineItems)
        {
            if (lineItems == null) return new LineItem[0];
            return lineItems
                .Where(x => x.ProductId.HasValue)
                .Select(lineItem => new LineItem
                {
                    Name = lineItem.Name,
                    ProductId = lineItem.ProductId.Value,
                    VariantId = lineItem.VariantId,
                    VariantTitle = lineItem.VariantTitle,
                    Properties = Convert(lineItem.Properties),
                }).ToArray();
        }

        private static LineItemProperty[] Convert(IEnumerable<ShopifyObjects.LineItemProperty> lineItemsProperties)
        {
            if (lineItemsProperties == null) return new LineItemProperty[0];
            return lineItemsProperties.Select(lineItem => new LineItemProperty
            {
                Name = lineItem.Name?.ToString() ?? string.Empty,
                Value = lineItem.Value,
            }).ToArray();
        }
    }
}
