namespace ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models
{
    public class Order
    {
        public long Id { get; set; }
        public LineItem[] LineItems { get; set; }
        public string Name { get; set; }
        public string ShippingAddressCountryCode { get; set; }
        public string Notes { get; set; }
    }
}
