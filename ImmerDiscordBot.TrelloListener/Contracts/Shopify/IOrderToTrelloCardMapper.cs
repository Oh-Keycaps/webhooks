using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;

namespace ImmerDiscordBot.TrelloListener.Contracts.Shopify
{
    public interface IOrderToTrelloCardMapper
    {
        TrelloCardToCreate MapToTrelloCard(Order order);
    }
}
