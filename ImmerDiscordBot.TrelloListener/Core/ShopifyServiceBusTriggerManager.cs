using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;

namespace ImmerDiscordBot.TrelloListener.Core
{
    public class ShopifyServiceBusTriggerManager
    {
        private readonly IOrderReader _orderReader;
        private readonly IOrderFilter _orderCreatedFilter;
        private readonly ILogger _logger;

        public ShopifyServiceBusTriggerManager(IOrderReader orderReader, IOrderFilter orderCreatedFilter, ILogger<ShopifyServiceBusTriggerManager> logger)
        {
            _orderReader = orderReader;
            _orderCreatedFilter = orderCreatedFilter;
            _logger = logger;
        }

        public IsOrderForDactylKeyboardResponse HandleMessage(Message m)
        {
            var order = _orderReader.ReadFromMessage(m);
            _logger.LogInformation("parsed order number:{0}", order.Name);
            var response = new IsOrderForDactylKeyboardResponse();
            if (_orderCreatedFilter.IsOrderForDactylKeyboard(order))
            {
                response.ShouldCreateCardOnTrello = true;
                response.ShouldAddRowToGoogleSheets = true;
                _logger.LogInformation("order {0} is a dactyl keyboard", order.Name);
            }
            return response;
        }
    }
    public struct IsOrderForDactylKeyboardResponse
    {
        public bool ShouldCreateCardOnTrello;
        public bool ShouldAddRowToGoogleSheets;
    }
}
