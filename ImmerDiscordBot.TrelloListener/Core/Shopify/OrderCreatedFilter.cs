using System.Linq;
using ImmerDiscordBot.TrelloListener.ShopifyObjects;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public class OrderCreatedFilter
    {
        private const long BuiltToOrderDactyl = 3874182594671;

        public bool IsOrderForDactylKeyboard(Order order)
        {
            return order.LineItems
                .Any(x => x.ProductId == BuiltToOrderDactyl);
        }
    }
}
