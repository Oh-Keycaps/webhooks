using System.Collections.Generic;
using System.Linq;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;
using ImmerDiscordBot.TrelloListener.ShopifyObjects;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public class OrderToTrelloCardMapper : IOrderToTrelloCardMapper
    {
        public TrelloCardToCreate MapToTrelloCard(Order order)
        {
            var builtToOrderDactyl = order.LineItems
                .Where(x => x.ProductId.HasValue)
                .First(x => ProductIdConstants.BuiltToOrderDactyl.Contains(x.ProductId.Value));
            var props = builtToOrderDactyl.Properties.ToArray();

            var trelloCardToCreate = new TrelloCardToCreate
            {
                OrderName = order.Name,
                Switches = GetPropertyByNameContains(props, "Switches"),
                MCU = GetPropertyByNameEquals(props, "Micro Controller Type"),
                CaseColor = GetPropertyByNameEquals(props, "Case Color"),
                CaseVariant = GetPropertyByNameEquals(props, "Dactyl/Manuform Layout"),
                WristRestColor = GetPropertyByNameEquals(props, "Gel Wrist Rest Color"),
                LEDs = GetPropertyByNameEquals(props, "LEDs (optional)"),
                IsDomestic = order.ShippingAddress.CountryCode.Equals("US"),
                Accessories = ExtractAccessories(order, props).ToArray(),
                PaintCaseColor = order.LineItems.FirstOrDefault(x => x.ProductId == ProductIdConstants.PaintCaseColorProductId)?.VariantTitle,
                IsBluetooth = order.LineItems.Any(x => x.ProductId == ProductIdConstants.BluetoothUpgradeProductId),
            };
            return trelloCardToCreate;
        }

        private static string[] ExtractAccessories(Order order, LineItemProperty[] props)
        {
            var accessories = new List<string>();
            AddAccessoryIfExists(order, ProductIdConstants.UsbCableProductId, accessories);
            AddAccessoryIfExists(order, ProductIdConstants.TrrsCableProductId, accessories);
            //getting keycaps name from properties because it is cleaner. If i get it from ProductId the name is really long.
            if (order.LineItems.Any(x => x.ProductId == ProductIdConstants.KeycapsProductId))
            {
                var name = GetPropertyByNameContains(props, "Keycaps");
                accessories.Add($"Keycaps - {name}");
            }

            return accessories.ToArray();
        }

        private static void AddAccessoryIfExists(Order order, long productId, ICollection<string> accessories)
        {
            var product = order.LineItems.FirstOrDefault(x => x.ProductId == productId);
            if (product != null) accessories.Add(product.Name);
        }

        private static string GetPropertyByNameEquals(LineItemProperty[] props, string propName)
        {
            return props.FirstOrDefault(x => x.Name.Equals(propName))?.Value.ToString();
        }

        private static string GetPropertyByNameContains(LineItemProperty[] props, string propName)
        {
            return props.FirstOrDefault(x => x.Name.ToString().Contains(propName))?.Value.ToString();
        }
    }
}
