using System.IO;
using System.Text;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using ImmerDiscordBot.TrelloListener.ShopifyObjects;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public class OrderJsonReader : IOrderReader
    {
        private readonly JsonSerializer _serializer;

        public OrderJsonReader()
        {
            _serializer = new JsonSerializer();
        }

        public Order ReadFromMessage(Message message)
        {
            var reader = new StreamReader(new MemoryStream(message.Body), Encoding.UTF8);
            using var jsonTextReader = new JsonTextReader(reader);
            return _serializer.Deserialize<Order>(jsonTextReader);
        }
    }
}
