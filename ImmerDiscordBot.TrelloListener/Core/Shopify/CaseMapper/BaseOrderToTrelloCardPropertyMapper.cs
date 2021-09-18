using System.Collections.Generic;
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
        public virtual string[] Accessories { get; protected set; }
        public virtual string PaintCaseColor  => _order.LineItems.FirstOrDefault(x => x.ProductId == ProductIdConstants.PaintCaseColorProductId)?.VariantTitle;
        public virtual bool IsDomestic => _order.ShippingAddressCountryCode.Equals("US");
        public virtual bool IsBluetooth => _order.LineItems.Any(x => x.ProductId == ProductIdConstants.BluetoothUpgradeProductId);
        public virtual CaseTypes CaseType => _caseTypes;

        protected Order _order;
        protected LineItem _builtToOrderDactyl;
        private CaseTypes _caseTypes;

        public void Bind(Order order, CaseTypes caseType)
        {
            _order = order;
            _caseTypes = caseType;
            _builtToOrderDactyl = order.GetBuiltToOrderDactyl();
            Accessories = ExtractAccessories(order, _builtToOrderDactyl);
        }

        private static string[] ExtractAccessories(Order order, LineItem builtToOrderDactyl)
        {
            var accessories = new List<string>();
            AddAccessoryIfExists(order, ProductIdConstants.UsbCableProductId, accessories);
            AddAccessoryIfExists(order, ProductIdConstants.TrrsCableProductId, accessories);
            //getting keycaps name from properties because it is cleaner. If i get it from ProductId the name is really long.
            if (order.LineItems.Any(x => x.ProductId == ProductIdConstants.KeycapsProductId))
            {
                var name = builtToOrderDactyl.GetPropertyByNameContains("Keycaps");
                accessories.Add($"Keycaps - {name}");
            }

            return accessories.ToArray();
        }

        private static void AddAccessoryIfExists(Order order, long productId, ICollection<string> accessories)
        {
            var product = order.LineItems.FirstOrDefault(x => x.ProductId == productId);
            if (product != null) accessories.Add(product.Name);
        }
    }
}
