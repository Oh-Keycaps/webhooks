using System.Linq;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using ImmerDiscordBot.TrelloListener.ShopifyObjects;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public class OrderCreatedFilter : IOrderFilter
    {
        public bool IsOrderForDactylKeyboard(Order order)
        {
            return order.LineItems
                .Any(x => x.ProductId == Models.ProductIdConstants.BuiltToOrderDactyl);
        }
    }
}
