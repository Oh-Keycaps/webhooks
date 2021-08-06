using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using Microsoft.Azure.ServiceBus;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public static class MessageOrderExtensions
    {
        public static Order ToOrderObject(this Message m)
        {
            var reader = new OrderJsonReader();
            return reader.ReadFromMessage(m);
        }
    }
}
