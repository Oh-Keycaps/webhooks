using System;
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
            Accessories = ExtractAccessories(_builtToOrderDactyl);
        }

        private string[] ExtractAccessories(LineItem builtToOrderDactyl)
        {
            var accessories = new List<string>();
            AddAccessoryIfExists(ProductIdConstants.UsbCable1, accessories);
            AddAccessoryIfExists(ProductIdConstants.UsbCable2, accessories, item => $"USB-C cables - {item.VariantTitle}");
            AddAccessoryIfExists(ProductIdConstants.TrrsCableProductId, accessories);
            AddSaKeycaps(builtToOrderDactyl, accessories);
            AddDSaKeycaps(builtToOrderDactyl, accessories);
            AddAccessoryIfExists(ProductIdConstants.BottomPlateProductId, accessories);
            var optionalWristRest = _builtToOrderDactyl.GetPropertyByNameEquals("Optional Wrist Rest? ");
            if (optionalWristRest != null)
            {
                accessories.Add(optionalWristRest);
            }

            return accessories.ToArray();
        }

        private void AddSaKeycaps(LineItem builtToOrderDactyl, List<string> accessories)
        {
            //getting keycaps name from properties because it is cleaner. If i get it from ProductId the name is really long.
            var keycaps = _order.LineItems.FirstOrDefault(x => x.ProductId == ProductIdConstants.SAKeycapsProductId);
            if (keycaps != null)
            {
                var name = builtToOrderDactyl.GetPropertyByNameContains("Keycaps");
                if (string.IsNullOrEmpty(name))
                {
                    name = keycaps.VariantTitle;
                }

                accessories.Add($"Keycaps - {name}");
            }
        }

        private void AddDSaKeycaps(LineItem builtToOrderDactyl, List<string> accessories)
        {
            var keycaps = _order.LineItems.Where(x => x.ProductId == ProductIdConstants.DSAKeycapsProductId);
            foreach(var keycap in keycaps)
            {
                accessories.Add($"Keycaps - {keycap.Name}");
            }
        }

        private void AddAccessoryIfExists(long productId, ICollection<string> accessories) =>
            AddAccessoryIfExists(productId, accessories, product => product.Name);

        private void AddAccessoryIfExists(long productId, ICollection<string> accessories, Func<LineItem, string> propertyGetter)
        {
            var product = _order.LineItems.FirstOrDefault(x => x.ProductId == productId);
            if (product != null) accessories.Add(propertyGetter(product));
        }
    }
}
