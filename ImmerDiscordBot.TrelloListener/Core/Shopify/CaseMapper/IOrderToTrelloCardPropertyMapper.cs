using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify.CaseMapper
{
    public interface IOrderToTrelloCardPropertyMapper
    {
        string OrderName { get; }
        string Switches { get; }
        string MCU { get; }
        string CaseColor { get; }
        string CaseVariant { get; }
        string WristRestColor { get; }
        string LEDs { get; }
        string[] Accessories { get; }
        string PaintCaseColor { get; }
        bool IsDomestic { get; }
        bool IsBluetooth { get; }
        CaseTypes CaseType { get; }
        string Notes { get; }
        AccessoryListBuilder AccessoryListBuilder { get; }
        string ShopifyOrderUrl { get; }

        void Bind(Order order, CaseTypes caseType);
    }
}
