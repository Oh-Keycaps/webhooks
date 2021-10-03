using System;
using System.Collections.Generic;
using System.Linq;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify.CaseMapper
{
    public class AccessoryListBuilder
    {
        public string UsbCable { get; private set; }
        public string TrrsCable { get; private set; }
        public string[] KeyCaps { get; private set; }
        public string BottomPlate  { get; private set; }
        public string WristRest  { get; private set; }

        public void Bind(Order order, LineItem builtToOrderDactyl)
        {
            ExtractAccessories(builtToOrderDactyl, order);
        }

        private void ExtractAccessories(LineItem builtToOrderDactyl, Order order)
        {
            UsbCable = AddAccessoryIfExists(ProductIdConstants.UsbCable1, order) ??
                       AddAccessoryIfExists(ProductIdConstants.UsbCable2, item => $"USB-C cables - {item.VariantTitle}", order);

            TrrsCable = AddAccessoryIfExists(ProductIdConstants.TrrsCableProductId, order);

            var accessories = new List<string>(3);
            AddSaKeycaps(builtToOrderDactyl, accessories, order);
            AddDSaKeycaps(accessories, order);
            KeyCaps = accessories.ToArray();

            BottomPlate = AddAccessoryIfExists(ProductIdConstants.BottomPlateProductId, order);

            var optionalWristRest = builtToOrderDactyl.GetPropertyByNameEquals("Optional Wrist Rest? ");
            if (optionalWristRest != null)
            {
                WristRest = optionalWristRest;
            }
        }

        private static void AddSaKeycaps(LineItem builtToOrderDactyl, ICollection<string> accessories, Order order)
        {
            //getting keycaps name from properties because it is cleaner. If i get it from ProductId the name is really long.
            var keycaps = order.LineItems.FirstOrDefault(x => x.ProductId == ProductIdConstants.SAKeycapsProductId);
            if (keycaps != null)
            {
                var name = builtToOrderDactyl.GetPropertyByNameContains("Keycaps");
                if (string.IsNullOrEmpty(name))
                {
                    name = keycaps.VariantTitle;
                }

                accessories.Add($"Keycaps - {name}");
                return;
            }

            keycaps = order.LineItems.FirstOrDefault(x => x.ProductId == ProductIdConstants.SA4x6DactylManuformKeycaps);
            if (keycaps != null)
            {
                var name = builtToOrderDactyl.GetPropertyByNameContains("Keycaps");
                if (string.IsNullOrEmpty(name))
                {
                    name = keycaps.VariantTitle;
                }

                accessories.Add($"Keycaps - SA {name}");
                return;
            }
        }

        private static void AddDSaKeycaps(ICollection<string> accessories, Order order)
        {
            var keycaps = order.LineItems.Where(x => x.ProductId == ProductIdConstants.DSAKeycapsProductId);
            foreach(var keycap in keycaps)
            {
                accessories.Add($"Keycaps - {keycap.Name}");
            }
        }

        private static string AddAccessoryIfExists(long productId, Order order) => AddAccessoryIfExists(productId, product => product.Name, order);

        private static string AddAccessoryIfExists(long productId, Func<LineItem, string> propertyGetter, Order order)
        {
            var product = order.LineItems.FirstOrDefault(x => x.ProductId == productId);
            if (product != null)
            {
                return propertyGetter(product);
            }

            return null;
        }

        public string[] ToAccessories()
        {
            var l = new List<string>();
            l.Add(UsbCable);
            l.Add(TrrsCable);
            l.AddRange(KeyCaps);
            l.Add(BottomPlate);
            l.Add(WristRest);
            return l.Where(s => !string.IsNullOrEmpty(s)).ToArray();
        }
    }
}
