using ImmerDiscordBot.TrelloListener.ShopifyObjects;
using Microsoft.Azure.ServiceBus;

namespace ImmerDiscordBot.TrelloListener.Contracts.Shopify
{
    public interface IOrderReader
    {
        Order ReadFromMessage(Message message);
    }
}
