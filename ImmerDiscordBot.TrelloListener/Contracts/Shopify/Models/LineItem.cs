namespace ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models
{
    public class LineItem
    {
        public long ProductId { get; set; }
        public long? VariantId { get; set; }
        public string VariantTitle { get; set; }
        public string Name { get; set; }
        public LineItemProperty[] Properties { get; set; }
    }
}
