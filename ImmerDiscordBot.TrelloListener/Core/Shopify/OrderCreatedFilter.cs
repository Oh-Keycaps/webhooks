using System.Linq;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public class OrderCreatedFilter : IOrderFilter
    {
        public bool IsOrderForDactylKeyboard(Order order)
        {
            return order.LineItems
                .Any(x => ProductIdConstants.BuiltToOrderDactyl.Contains(x.ProductId));
        }
    }
}
