using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify.CaseMapper
{
    public class DiyOrderToTrelloCardPropertyMapper : BaseOrderToTrelloCardPropertyMapper
    {
        public override string CaseVariant => _builtToOrderDactyl.GetPropertyByNameEquals("Dactyl/Manuform Case");
        public override string MCU => _builtToOrderDactyl.GetPropertyByNameEquals("Elite Cs");
        public override string WristRestColor => _builtToOrderDactyl.GetPropertyByNameEquals("Gel Wrist Rest");
    }
}
