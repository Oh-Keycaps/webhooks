using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;

namespace ImmerDiscordBot.TrelloListener.Contracts.Shopify
{
    public interface IOrderConverter
    {
        Order Convert(ShopifyObjects.Order fullOrder);
    }
}
