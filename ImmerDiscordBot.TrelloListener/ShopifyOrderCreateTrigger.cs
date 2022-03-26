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
        public async Task<IActionResult> CallShopifyForOrder([HttpTrigger(AuthorizationLevel.Function, "post", Route = "CallShopify/orders/{orderId:long}")] HttpRequest req, long orderId,
            [ServiceBus("startshopify", EntityType = EntityType.Queue)]
            IAsyncCollector<Order> messageCollector,
            ILogger log, ILogger logger)
        {
            try
            {
                var fullOrder = await _shopifyClient.GetOrder(orderId);
                using (log.UseScope(fullOrder))
                {
                    var order = _converter.Convert(fullOrder, logger);
                    await messageCollector.AddAsync(order);
                    return new OkResult();
                }
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

        /// <summary> Listens to a webhook from Shopify when a order is created. </summary>
        /// <param name="req">The posted message from Shopify. Contains PII, must remove as it isn't needed for processing</param>
        /// <param name="messageCollector">collects a clean Order with only required parameters. </param>
        /// <param name="errorMessageCollector">if there is a problem when parsing we send the message to error queue</param>
        /// <param name="log">function logger</param>
        /// <param name="token"> CancellationToken...it is what it is </param>
        /// <returns>Shopify wants to know if I heard it. If I don't send ok and enough messages fail it will stop sending messages to me</returns>
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

            using (log.UseScope(fullOrder))
            {
                try
                {
                    if (!string.IsNullOrEmpty(fullOrder.CancelReason))
                    {
                        return new OkResult();
                    }

                    var order = _converter.Convert(fullOrder, log);
                    await messageCollector.AddAsync(order, token);

                    return new OkResult();
                }
                catch (NullReferenceException e)
                {
                    await MoveMessageToErrorQueue(errorMessageCollector, token, e, fullOrder);
                    //swallow exception because these are in error queue and can be retried
                    return new OkResult();
                }
                catch (Exception e)
                {
                    await MoveMessageToErrorQueue(errorMessageCollector, token, e, fullOrder);
                    throw;
                }
            }
        }

        private static async Task MoveMessageToErrorQueue(IAsyncCollector<ErrorContext> errorMessageCollector, CancellationToken token, Exception e, ShopifyObjects.Order fullOrder)
        {
            var errorContext = new ErrorContext
            {
                ErrorMessage = e.Message,
                Exception = e,
                Order = fullOrder,
            };
            await errorMessageCollector.AddAsync(errorContext, token);
            await errorMessageCollector.FlushAsync(token);
        }

        public class ErrorContext
        {
            public string ErrorMessage { get; set; }
            public Exception Exception { get; set; }
            public ShopifyObjects.Order Order { get; set; }
        }
    }

    public static class ShopifyOrderCreateTriggerLoggingExtensions
    {
        private static readonly Func<ILogger, string, long?, IDisposable> _scopeAction  = LoggerMessage.DefineScope<string, long?>("Working with order {name} id {id}");

        public static IDisposable UseScope(this ILogger logger, ShopifyObjects.Order order)
        {
            return _scopeAction(logger, order.Name, order.Id);
        }
    }
}
