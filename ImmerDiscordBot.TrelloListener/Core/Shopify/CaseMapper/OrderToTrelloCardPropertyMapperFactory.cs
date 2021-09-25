using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify.CaseMapper
{
    public static class OrderToTrelloCardPropertyMapperFactory
    {
        public static IOrderToTrelloCardPropertyMapper Create(Order order, CaseTypes caseType)
        {
            IOrderToTrelloCardPropertyMapper propertyMapper = caseType switch
            {
                CaseTypes.DIY => new DiyOrderToTrelloCardPropertyMapper(),
                _ => new BaseOrderToTrelloCardPropertyMapper(),
            };
            propertyMapper.Bind(order, caseType);
            return propertyMapper;
        }
    }
}
