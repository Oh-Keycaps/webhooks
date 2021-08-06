using System.Threading;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using ImmerDiscordBot.TrelloListener.Core.Trello;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ImmerDiscordBot.TrelloListener.CreateCardOnTrelloCore
{
    public class CreateCardOnTrelloHandler
    {
        private readonly IOrderToTrelloCardMapper _orderMapper;
        private readonly TrelloClient _trelloClient;

        public CreateCardOnTrelloHandler(TrelloClient trelloClient, IOrderToTrelloCardMapper orderMapper)
        {
            _trelloClient = trelloClient;
            _orderMapper = orderMapper;
        }

        [Disable("DisableCreateTrelloCardFunction"),FunctionName("CreateTrelloCardFunction")]
        public async Task HandleMessage([ServiceBusTrigger("createtrellocard")] Order order, ILogger log, CancellationToken token)
        {
            log.LogTrace("+HandleMessage {0}", order.Name);
            var trelloCardToCreate = _orderMapper.MapToTrelloCard(order);
            await _trelloClient.CreateCard(trelloCardToCreate, token);
            log.LogTrace("-HandleMessage {0}",  order.Name);
        }
    }
}
