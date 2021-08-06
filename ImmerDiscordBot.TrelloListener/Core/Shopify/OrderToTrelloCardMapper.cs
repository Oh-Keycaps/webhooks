using System.Collections.Generic;
using System.Linq;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public class OrderToTrelloCardMapper : IOrderToTrelloCardMapper
    {
        public TrelloCardToCreate MapToTrelloCard(Order order)
        {
            var builtToOrderDactyl = order.GetBuiltToOrderDactyl();
            var caseType = builtToOrderDactyl.GetCaseType();
            var trelloCardToCreate = new TrelloCardToCreate
            {
                OrderName = order.Name,
                Switches = builtToOrderDactyl.GetPropertyByNameContains("Switches"),
                MCU = builtToOrderDactyl.GetPropertyByNameEquals("Micro Controller Type"),
                CaseColor = builtToOrderDactyl.GetPropertyByNameEquals("Case Color"),
                CaseVariant = builtToOrderDactyl.GetPropertyByNameEquals("Dactyl/Manuform Layout"),
                WristRestColor = builtToOrderDactyl.GetPropertyByNameEquals("Gel Wrist Rest Color"),
                LEDs = builtToOrderDactyl.GetPropertyByNameEquals("LEDs (optional)"),
                IsDomestic = order.ShippingAddressCountryCode.Equals("US"),
                Accessories = ExtractAccessories(order, builtToOrderDactyl).ToArray(),
                PaintCaseColor = order.LineItems.FirstOrDefault(x => x.ProductId == ProductIdConstants.PaintCaseColorProductId)?.VariantTitle,
                IsBluetooth = order.LineItems.Any(x => x.ProductId == ProductIdConstants.BluetoothUpgradeProductId),
                CaseType = caseType,
            };
            return trelloCardToCreate;
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
