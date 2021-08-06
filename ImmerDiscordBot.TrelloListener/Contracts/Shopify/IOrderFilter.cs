using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;

namespace ImmerDiscordBot.TrelloListener.Contracts.Shopify
{
    public interface IOrderFilter
    {
        bool IsOrderForDactylKeyboard(Order order);
    }
}
