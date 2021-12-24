using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify.CaseMapper
{
    public class PrimeagenDactylPropertyMapper : BaseOrderToTrelloCardPropertyMapper
    {
        public override string CaseVariant => "Dactyl";
        public override string Switches => "Kailh Box Jade";
        public override string CaseColor => "White";
        public override CaseTypes CaseType => CaseTypes.PETG_PLA;
        public override string MCU => "Elite C";
        public override string WristRestColor => "Black";
        public override void Bind(Order order, CaseTypes caseType)
        {
            base.Bind(order, caseType);
            AccessoryListBuilder.KeyCaps = new[]
            {
                "SA Blue/Black/White blank keycaps",
                "Red Escape Key",
                "Black Scooped home keys"
            };
            AccessoryListBuilder.TrrsCable = "TRRS Cables - Blue/Black";
            AccessoryListBuilder.UsbCable = "USB-C Blue/Black coiled cable";
        }
    }
}
