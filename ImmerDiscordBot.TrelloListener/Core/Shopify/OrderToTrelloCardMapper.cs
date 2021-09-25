using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using ImmerDiscordBot.TrelloListener.Core.Shopify.CaseMapper;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public class OrderToTrelloCardMapper : IOrderToTrelloCardMapper
    {
        public TrelloCardToCreate MapToTrelloCard(Order order)
        {
            var builtToOrderDactyl = order.GetBuiltToOrderDactyl();
            var caseType = builtToOrderDactyl.GetCaseType();
            var propertyMapper = OrderToTrelloCardPropertyMapperFactory.Create(order, caseType);
            var trelloCardToCreate = new TrelloCardToCreate
            {
                OrderName = propertyMapper.OrderName,
                Switches = propertyMapper.Switches,
                MCU = propertyMapper.MCU,
                CaseColor = propertyMapper.CaseColor,
                CaseVariant = propertyMapper.CaseVariant,
                WristRestColor = propertyMapper.WristRestColor,
                LEDs = propertyMapper.LEDs,
                IsDomestic = propertyMapper.IsDomestic,
                Accessories = propertyMapper.Accessories,
                PaintCaseColor = propertyMapper.PaintCaseColor,
                IsBluetooth = propertyMapper.IsBluetooth,
                CaseType = propertyMapper.CaseType,
            };
            return trelloCardToCreate;
        }
    }
}
