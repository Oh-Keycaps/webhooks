using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;

namespace ImmerDiscordBot.TrelloListener.Core.GoogleSheets
{
    public class OrderToSheetRowMapper
    {
        public SheetRow MapToSheetRow(Order order)
        {
            var builtToOrderDactyl = order.GetBuiltToOrderDactyl();
            var caseType = builtToOrderDactyl.GetCaseType();
            var trelloCardToCreate = new SheetRow
            {
                OrderName = order.Name.TrimStart('#'),
                MCU = builtToOrderDactyl.GetPropertyByNameEquals("Micro Controller Type"),
                CaseColor = builtToOrderDactyl.GetPropertyByNameEquals("Case Color"),
                CaseVariant = builtToOrderDactyl.GetPropertyByNameEquals("Dactyl/Manuform Layout"),
                CaseType = caseType.ToString(),
            };
            return trelloCardToCreate;
        }
    }
}
