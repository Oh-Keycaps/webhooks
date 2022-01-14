using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public static class MessageOrderExtensions
    {
        public static Order ToOrderObject(this HttpRequest request, ILogger logger)
        {
            var reader = new OrderJsonReader();
            var converter = new OrderConverter();
            var fullOrder = reader.ReadFromStream(request.Body);
            return converter.Convert(fullOrder, logger);
        }
    }
}
