using System;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly ShopifyServiceBusTriggerManager _manager;

        public ShopifyServiceBusTrigger(ShopifyServiceBusTriggerManager manager)
        {
            _manager = manager;
        }

        [FunctionName(nameof(ShopifyServiceBusTrigger))]
        public async Task Trigger([ServiceBusTrigger("startshopify")] Message m, ILogger log, CancellationToken token,
            [ServiceBus("startshopify.error", EntityType = EntityType.Queue)]
            IAsyncCollector<Message> messageCollector,
            [ServiceBus("createtrellocard", EntityType = EntityType.Queue)]
            IAsyncCollector<Message> createTrelloCardCollector,
            [ServiceBus("createrowgooglesheets", EntityType = EntityType.Queue)]
            IAsyncCollector<Message> createRowGoogleSheetsCollector
        )
        {
            log.LogDebug("+Processing message:{MessageId}", m.MessageId);
            try
            {
                var response = _manager.HandleMessage(m);
                var message = m.Clone();
                if (response.ShouldCreateCardOnTrello)
                {
                    log.LogInformation("Sending message to createtrellocard queue for processing");
                    await createTrelloCardCollector.AddAsync(message, token);
                    await createTrelloCardCollector.FlushAsync(token);
                }

                if (response.ShouldAddRowToGoogleSheets)
                {
                    log.LogInformation("Sending message to createrowgooglesheets queue for processing");
                    await createRowGoogleSheetsCollector.AddAsync(message, token);
                    await createRowGoogleSheetsCollector.FlushAsync(token);
                }
            }
            catch (Exception e)
            {
                log.LogError(e, "Error while processing message");
                var erroredMessage = m.Clone();
                erroredMessage.UserProperties.Add("Exception", JsonConvert.SerializeObject(e));
                await messageCollector.AddAsync(erroredMessage, token);
            }

            log.LogDebug("-Processing message:{MessageId}", m.MessageId);
        }
    }
}
