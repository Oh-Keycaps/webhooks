using System.IO;
using ImmerDiscordBot.TrelloListener.ShopifyObjects;
using Microsoft.Azure.ServiceBus;
using ContractOrder = ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models.Order;

namespace ImmerDiscordBot.TrelloListener.Contracts.Shopify
{
    public interface IOrderReader
    {
        ContractOrder ReadFromMessage(Message message);
        Order ReadFromStream(Stream stream);
    }
}
