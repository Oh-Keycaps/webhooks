using System.Threading;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;
using ImmerDiscordBot.TrelloListener.Core.Trello;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;

namespace ImmerDiscordBot.TrelloListener.Core
{
    public class ShopifyServiceBusTriggerManager
    {
        private readonly IOrderReader _orderReader;
        private readonly IOrderFilter _orderCreatedFilter;
        private readonly IOrderToTrelloCardMapper _orderMapper;
        private readonly TrelloClient _trelloClient;
        private readonly ILogger<ShopifyServiceBusTriggerManager> _logger;

        public ShopifyServiceBusTriggerManager(IOrderReader orderReader, IOrderFilter orderCreatedFilter, IOrderToTrelloCardMapper orderMapper, TrelloClient trelloClient, ILogger<ShopifyServiceBusTriggerManager> logger)
        {
            _orderReader = orderReader;
            _orderCreatedFilter = orderCreatedFilter;
            _orderMapper = orderMapper;
            _trelloClient = trelloClient;
            _logger = logger;
        }

        public async Task HandleMessage(Message m, CancellationToken token)
        {
            var order = _orderReader.ReadFromMessage(m);
            _logger.LogInformation("parsed order number:{0}", order.Name);
            if (_orderCreatedFilter.IsOrderForDactylKeyboard(order))
            {
                _logger.LogInformation("order {0} is a dactyl keyboard", order.Name);
                var trelloCardToCreate = _orderMapper.MapToTrelloCard(order);
                await Task.WhenAll
                (
                    CreateCardOnTrello(trelloCardToCreate, token)
                );
            }
        }

        private async Task CreateCardOnTrello(TrelloCardToCreate trelloCardInfo, CancellationToken token)
        {
            _logger.LogDebug("+CreateCardOnTrello {0}", trelloCardInfo.OrderName);
            await _trelloClient.CreateCard(trelloCardInfo, token);
            _logger.LogDebug("-CreateCardOnTrello {0}", trelloCardInfo.OrderName);
        }
    }
}
