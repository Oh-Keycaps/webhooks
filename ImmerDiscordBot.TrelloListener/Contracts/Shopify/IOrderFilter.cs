using ImmerDiscordBot.TrelloListener.ShopifyObjects;

namespace ImmerDiscordBot.TrelloListener.Contracts.Shopify
{
    public interface IOrderFilter
    {
        bool IsOrderForDactylKeyboard(Order order);
    }
}
