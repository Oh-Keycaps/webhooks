using System.Linq;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;
using ImmerDiscordBot.TrelloListener.ShopifyObjects;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public class OrderCreatedFilter : IOrderFilter
    {
        public bool IsOrderForDactylKeyboard(Order order)
        {
            return order.LineItems
                .Where(x => x.ProductId.HasValue)
                .Any(x => ProductIdConstants.BuiltToOrderDactyl.Contains(x.ProductId.Value));
        }
    }
}
