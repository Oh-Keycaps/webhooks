using System;
using System.Threading;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using ImmerDiscordBot.TrelloListener.Core;
using ImmerDiscordBot.TrelloListener.Core.Trello;
using ImmerDiscordBot.TrelloListener.ShopifyObjects;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener
{
    public class ShopifyServiceBusTrigger
    {
        private readonly ShopifyServiceBusTriggerManager _manager;
        public ShopifyServiceBusTrigger(ShopifyServiceBusTriggerManager manager)
        {
            _manager = manager;
        }

        [FunctionName(nameof(ShopifyServiceBusTrigger))]
        public async Task Trigger([ServiceBusTrigger("startshopify")] Message m, ILogger log, CancellationToken token,
            [ServiceBus("startshopify.error", EntityType = EntityType.Queue)]IAsyncCollector<Message> messageCollector
            )
        {
            log.LogInformation("+Processing message:{0}", m.MessageId);
            try
            {
                await _manager.HandleMessage(m, token);
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
