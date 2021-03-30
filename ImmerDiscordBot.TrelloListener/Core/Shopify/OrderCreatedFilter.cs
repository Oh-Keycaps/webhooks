using System.Linq;
using ImmerDiscordBot.TrelloListener.ShopifyObjects;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public class OrderCreatedFilter
    {
        public bool IsOrderForDactylKeyboard(Order order)
        {
            return order.LineItems
                .Any(x => x.ProductId == Models.ProductIdConstants.BuiltToOrderDactyl);
        }
    }
}
