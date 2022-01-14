using System;
using System.Collections.Generic;
using System.Linq;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using Microsoft.Extensions.Logging;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public class OrderConverter : IOrderConverter
    {
        public Order Convert(ShopifyObjects.Order order, ILogger logger)
        {
            logger.LogInformation("starting to convert order");
            var convert = new Order();
            try
            {
                convert.Id = order.Id.GetValueOrDefault(-1);
                convert.Name = order.Name;
                convert.ShippingAddressCountryCode = order.ShippingAddress.CountryCode;
                convert.LineItems = Convert(order.LineItems, logger);
                convert.Notes = order.Note;
            }
            catch (Exception e)
            {
                logger.LogError(e, "problem while converting order");
                throw;
            }
            return convert;
        }

        private static LineItem[] Convert(IEnumerable<ShopifyObjects.LineItem> lineItems, ILogger logger)
        {
            if (lineItems == null)
            {
                logger.LogDebug("No line items found");
                return Array.Empty<LineItem>();
            }
            var convert = lineItems
                .Where(x => x.ProductId.HasValue)
                .Select(item => SelectLineItem(item, logger)).ToArray();
            return convert;
        }

        private static LineItem SelectLineItem(ShopifyObjects.LineItem lineItem, ILogger logger)
        {
            var item = new LineItem();
            try
            {
                item.Name = lineItem.Name;
                item.ProductId = lineItem.ProductId.Value;
                item.VariantId = lineItem.VariantId;
                item.VariantTitle = lineItem.VariantTitle;
                item.Properties = Convert(lineItem.Properties, logger);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Problem converting line item {Id} in order", lineItem.Id);
                throw;
            }
            return item;
        }

        private static LineItemProperty[] Convert(IEnumerable<ShopifyObjects.LineItemProperty> lineItemsProperties, ILogger logger)
        {
            if (lineItemsProperties == null)
            {
                logger.LogDebug("line item properties are empty");
                return Array.Empty<LineItemProperty>();
            }

            return lineItemsProperties.Select(lineItem => SelectLineItemProperty(lineItem, logger)).ToArray();
        }

        private static LineItemProperty SelectLineItemProperty(ShopifyObjects.LineItemProperty lineItem, ILogger logger)
        {
            var property = new LineItemProperty();
            try
            {
                property.Name = lineItem.Name?.ToString() ?? string.Empty;
                property.Value = lineItem.Value;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Problem converting line item property {Name} in order", lineItem.Name);
                throw;
            }
            return property;
        }
    }
}
