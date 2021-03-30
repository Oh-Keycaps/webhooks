using System;
using System.Threading;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using ImmerDiscordBot.TrelloListener.Core;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener
{
    public class ShopifyServiceBusTrigger
    {
        private readonly IOrderReader _orderReader;
        private readonly IOrderFilter _orderCreatedFilter;
        private readonly IOrderToTrelloCardMapper _orderMapper;
        private readonly TrelloClient _trelloClient;

        public ShopifyServiceBusTrigger(IOrderReader orderReader, IOrderFilter orderCreatedFilter, IOrderToTrelloCardMapper orderMapper, TrelloClient trelloClient)
        {
            _orderReader = orderReader;
            _orderCreatedFilter = orderCreatedFilter;
            _orderMapper = orderMapper;
            _trelloClient = trelloClient;
        }

        [FunctionName(nameof(ShopifyServiceBusTrigger))]
        public async Task Trigger([ServiceBusTrigger("startshopify")] Message m, ILogger log, CancellationToken token,
            [ServiceBus("startshopify.error", EntityType = EntityType.Queue)]IAsyncCollector<Message> messageCollector
            )
        {
            log.LogInformation("+Processing message:{0}", m.MessageId);
            try
            {
                var order = _orderReader.ReadFromMessage(m);
                if (_orderCreatedFilter.IsOrderForDactylKeyboard(order))
                {
                    log.LogInformation("Creating trello card for order {0}", order.Name);
                    var trelloCardInfo = _orderMapper.MapToTrelloCard(order);
                    await _trelloClient.CreateCard(trelloCardInfo, token);
                    log.LogInformation("Created trello card for order {0}", order.Name);
                }
            }
            catch (Exception e)
            {
                log.LogError(e, "Error while processing message");
                var erroredMessage = m.Clone();
                erroredMessage.UserProperties.Add("Exception", JsonConvert.SerializeObject(e));
                await messageCollector.AddAsync(erroredMessage, token);
            }
            log.LogInformation("-Processing message:{0}", m.MessageId);
        }
    }
}
