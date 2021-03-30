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
            var builtToOrderDactyl = order.LineItems.First(x => x.ProductId == ProductIdConstants.BuiltToOrderDactyl);
            var props = builtToOrderDactyl.Properties.ToArray();
            var accessories = new List<string>();
            AddAccessoryIfExists(order, ProductIdConstants.UsbCableProductId, accessories);
            AddAccessoryIfExists(order, ProductIdConstants.TrrsCableProductId, accessories);

            return new TrelloCardToCreate
            {
                OrderName = order.Name,
                Switches = GetPropertyByNameContains(props, "Switches"),
                MCU = GetPropertyByNameEquals(props, "Micro Controller Type"),
                CaseColor = GetPropertyByNameEquals(props, "Case Color"),
                CaseVariant = GetPropertyByNameEquals(props, "Dactyl/Manuform Layout"),
                WristRestColor = GetPropertyByNameEquals(props, "Gel Wrist Rest Color"),
                LEDs = GetPropertyByNameEquals(props, "LEDs (optional)"),
                IsDomestic = order.ShippingAddress.CountryCode.Equals("US"),
                IsBluetooth = !string.IsNullOrEmpty(GetPropertyByNameEquals(props, "Bluetooth Upgrade*")),
                Accessories = accessories.ToArray()
            };
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
