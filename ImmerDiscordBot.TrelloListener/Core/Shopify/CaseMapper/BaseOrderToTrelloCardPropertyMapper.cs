using System.Linq;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify.CaseMapper
{
    public class BaseOrderToTrelloCardPropertyMapper : IOrderToTrelloCardPropertyMapper
    {
        public virtual string OrderName => _order.Name;
        public virtual string Switches => _builtToOrderDactyl.GetPropertyByNameContains("Switches");
        public virtual string MCU => _builtToOrderDactyl.GetPropertyByNameEquals("Micro Controller Type");
        public virtual string CaseColor => _builtToOrderDactyl.GetPropertyByNameEquals("Case Color");
        public virtual string CaseVariant => _builtToOrderDactyl.GetPropertyByNameEquals("Dactyl/Manuform Layout");
        public virtual string WristRestColor  => _builtToOrderDactyl.GetPropertyByNameEquals("Gel Wrist Rest Color");
        public virtual string LEDs  => _builtToOrderDactyl.GetPropertyByNameEquals("LEDs (optional)");
        public virtual string[] Accessories => AccessoryListBuilder.ToAccessories();
        public virtual string PaintCaseColor  => _order.LineItems.FirstOrDefault(x => x.ProductId == ProductIdConstants.PaintCaseColorProductId)?.VariantTitle;
        public virtual bool IsDomestic => _order.ShippingAddressCountryCode.Equals("US");
        public virtual bool IsBluetooth => _order.LineItems.Any(x => x.ProductId == ProductIdConstants.BluetoothUpgradeProductId);
        public virtual CaseTypes CaseType => _caseTypes;
        public virtual string Notes => _order.Notes;
        public virtual string ShopifyOrderUrl => $"https://mechcaps.myshopify.com/admin/orders/{_order.Id}";
        public AccessoryListBuilder AccessoryListBuilder { get; private set; }

        protected Order _order;
        protected LineItem _builtToOrderDactyl;
        private CaseTypes _caseTypes;

        public virtual void Bind(Order order, CaseTypes caseType)
        {
            _order = order;
            _caseTypes = caseType;
            _builtToOrderDactyl = order.GetBuiltToOrderDactyl();
            AccessoryListBuilder = new AccessoryListBuilder();
            AccessoryListBuilder.Bind(order, _builtToOrderDactyl);
        }
    }
}
