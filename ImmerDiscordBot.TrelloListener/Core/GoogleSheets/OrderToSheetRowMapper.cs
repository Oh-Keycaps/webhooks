using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using ImmerDiscordBot.TrelloListener.Core.Shopify.CaseMapper;

namespace ImmerDiscordBot.TrelloListener.Core.GoogleSheets
{
    public class OrderToSheetRowMapper
    {
        public SheetRow MapToSheetRow(Order order)
        {
            var builtToOrderDactyl = order.GetBuiltToOrderDactyl();
            var caseType = builtToOrderDactyl.GetCaseType();
            var propertyMapper = OrderToTrelloCardPropertyMapperFactory.Create(order, caseType);

            var sheetRow = new SheetRow
            {
                OrderName = propertyMapper.OrderName.TrimStart('#'),
                MCU = propertyMapper.MCU,
                CaseColor = propertyMapper.CaseColor,
                CaseVariant = propertyMapper.CaseVariant,
                CaseType = caseType.ToString(),
                Notes = propertyMapper.Notes,
                WristRestsIncluded = propertyMapper.AccessoryListBuilder.WristRest
            };
            return sheetRow;
        }
    }
}
