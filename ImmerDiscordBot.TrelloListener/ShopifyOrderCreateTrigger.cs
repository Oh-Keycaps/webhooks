using System;
using System.Threading;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using ImmerDiscordBot.TrelloListener.Core.Shopify;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using ShopifyLineItem = ImmerDiscordBot.TrelloListener.ShopifyObjects.LineItem;
using ShopifyLineItemProperty = ImmerDiscordBot.TrelloListener.ShopifyObjects.LineItemProperty;

namespace ImmerDiscordBot.TrelloListener
{
    public class ShopifyOrderCreateTrigger
    {
        private readonly IOrderReader _reader;
        private readonly IOrderConverter _converter;
        private readonly IShopifyClient _shopifyClient;

        public ShopifyOrderCreateTrigger(IOrderReader reader, IShopifyClient shopifyClient, IOrderConverter converter)
        {
            _reader = reader;
            _shopifyClient = shopifyClient;
            _converter = converter;
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
                var order = _converter.Convert(fullOrder);
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
            [ServiceBus("startshopify.error", EntityType = EntityType.Queue)]
            IAsyncCollector<ErrorContext> errorMessageCollector,
            ILogger log, CancellationToken token)
        {
            var fullOrder = _reader.ReadFromStream(req.Body);
            try
            {
                if (!string.IsNullOrEmpty(fullOrder.CancelReason))
                {
                    return new OkResult();
                }

                var order = _converter.Convert(fullOrder);
                await messageCollector.AddAsync(order, token);

                return new OkResult();
            }
            catch (Exception e)
            {
                var errorContext = new ErrorContext
                {
                    ErrorMessage = e.Message,
                    Exception = e,
                    Order = fullOrder,
                };
                await errorMessageCollector.AddAsync(errorContext, token);
                await errorMessageCollector.FlushAsync(token);
                throw;
            }
        }

        public class ErrorContext
        {
            public string ErrorMessage { get; set; }
            public Exception Exception { get; set; }
            public ShopifyObjects.Order Order { get; set; }
        }
    }
}
