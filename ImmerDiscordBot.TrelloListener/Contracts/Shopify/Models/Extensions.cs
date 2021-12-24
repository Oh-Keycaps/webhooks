using System.Linq;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;

namespace ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models
{
    public static class Extensions
    {
        public static LineItem GetBuiltToOrderDactyl(this Order order)
        {
            return order.LineItems
                .First(x => ProductIdConstants.BuiltToOrderDactyl.Contains(x.ProductId));
        }

        public static CaseTypes GetCaseType(this LineItem builtToOrderDactyl)
        {
            var caseType = builtToOrderDactyl.ProductId switch
            {
                ProductIdConstants.BuiltToOrderDactylFdm => CaseTypes.PETG_PLA,
                ProductIdConstants.BuiltToOrderDactylSla => CaseTypes.SLA,
                ProductIdConstants.BuiltToOrderDactylDiy => CaseTypes.DIY,
                ProductIdConstants.ThePrimeagenDactyl => CaseTypes.Primeagen,
                _ => CaseTypes.UNKNOWN
            };
            return caseType;
        }

        public static string GetPropertyByNameEquals(this LineItem lineItem, string propName)
        {
            return lineItem.Properties.FirstOrDefault(x => x.Name.Equals(propName))?.Value.ToString();
        }

        public static string GetPropertyByNameContains(this LineItem lineItem, string propName)
        {
            return lineItem.Properties.FirstOrDefault(x => x.Name.ToString().Contains(propName))?.Value.ToString();
        }
    }
}
