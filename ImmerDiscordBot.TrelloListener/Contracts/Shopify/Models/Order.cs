namespace ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models
{
    public class Order
    {
        public LineItem[] LineItems { get; set; }
        public string Name { get; set; }
        public string ShippingAddressCountryCode { get; set; }
    }
}
