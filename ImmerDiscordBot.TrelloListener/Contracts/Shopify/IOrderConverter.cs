using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using Microsoft.Extensions.Logging;

namespace ImmerDiscordBot.TrelloListener.Contracts.Shopify
{
    public interface IOrderConverter
    {
        Order Convert(ShopifyObjects.Order fullOrder, ILogger logger);
    }
}
