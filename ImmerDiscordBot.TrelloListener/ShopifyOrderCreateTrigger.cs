using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using ImmerDiscordBot.TrelloListener.Core.Shopify;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using ShopifyOrder = ImmerDiscordBot.TrelloListener.ShopifyObjects.Order;
using ShopifyLineItem = ImmerDiscordBot.TrelloListener.ShopifyObjects.LineItem;
using ShopifyLineItemProperty = ImmerDiscordBot.TrelloListener.ShopifyObjects.LineItemProperty;

namespace ImmerDiscordBot.TrelloListener
{
    public class ShopifyOrderCreateTrigger
    {
        private readonly IOrderReader _reader;
        private readonly IShopifyClient _shopifyClient;

        public ShopifyOrderCreateTrigger(IOrderReader reader, IShopifyClient shopifyClient)
        {
            _reader = reader;
            _shopifyClient = shopifyClient;
        }

        [FunctionName("CallShopifyForOrder")]
        public async Task<IActionResult> CallShopifyForOrder(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "CallShopify/orders/{orderId:long}")]
            HttpRequest req, long orderId,
            [ServiceBus("startshopify", EntityType = EntityType.Queue)]
            IAsyncCollector<Order> messageCollector,
            ILogger log)
        {
            try
            {
                var fullOrder = await _shopifyClient.GetOrder(orderId);
                var order = Convert(fullOrder);
                await messageCollector.AddAsync(order);
                return new OkResult();
            }
            catch (Exception e)
            {
                log.LogError(e, "error processing stuff");
                return new ObjectResult(e)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [FunctionName("ShopifyOrderCreateTrigger")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequest req,
            [ServiceBus("startshopify", EntityType = EntityType.Queue)]
            IAsyncCollector<Order> messageCollector,
            ILogger log)
        {
            var fullOrder = _reader.ReadFromStream(req.Body);
            var order = Convert(fullOrder);
            await messageCollector.AddAsync(order);
            return new OkResult();
        }

        private static Order Convert(ShopifyOrder order)
        {
            return new Order
            {
                Name = order.Name,
                ShippingAddressCountryCode = order.ShippingAddress.CountryCode,
                LineItems = Convert(order.LineItems),
            };
        }

        private static LineItem[] Convert(IEnumerable<ShopifyLineItem> lineItems)
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

        private static LineItemProperty[] Convert(IEnumerable<ShopifyLineItemProperty> lineItemsProperties)
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
