using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.ServiceBus;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public static class MessageOrderExtensions
    {
        public static Order ToOrderObject(this HttpRequest request)
        {
            var reader = new OrderJsonReader();
            var converter = new OrderConverter();
            var fullOrder = reader.ReadFromStream(request.Body);
            return converter.Convert(fullOrder);
        }
        public static Order ToOrderObject(this Message m)
        {
            var reader = new OrderJsonReader();
            return reader.ReadFromMessage(m);
        }
    }
}
