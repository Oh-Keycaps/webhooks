using System.IO;
using System.Text;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using ImmerDiscordBot.TrelloListener.ShopifyObjects;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using ContractOrder = ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models.Order;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public class OrderJsonReader : IOrderReader
    {
        private readonly JsonSerializer _serializer;

        public OrderJsonReader()
        {
            _serializer = new JsonSerializer();
        }

        public ContractOrder ReadFromMessage(Message message)
        {
            return ReadFromStream<ContractOrder>(new MemoryStream(message.Body));
        }

        public Order ReadFromStream(Stream stream)
        {
            return ReadFromStream<Order>(stream);
        }

        private T ReadFromStream<T>(Stream stream) where T : class
        {
            var reader = new StreamReader(stream, Encoding.UTF8);
            using var jsonTextReader = new JsonTextReader(reader);
            return _serializer.Deserialize<T>(jsonTextReader);
        }
    }
}
