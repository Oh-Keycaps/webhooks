using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;
using ImmerDiscordBot.TrelloListener.ShopifyObjects;

namespace ImmerDiscordBot.TrelloListener.Contracts.Shopify
{
    public interface IOrderToTrelloCardMapper
    {
        TrelloCardToCreate MapToTrelloCard(Order order);
    }
}
