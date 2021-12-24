namespace ImmerDiscordBot.TrelloListener.Core.Shopify.Models
{
    public class TrelloCardToCreate
    {
        public string OrderName { get; set; }
        public string CaseColor { get; set; }
        public string CaseVariant { get; set; }
        public string Switches { get; set; }
        public string WristRestColor { get; set; }
        public string MCU { get; set; }
        public string LEDs { get; set; }
        public string PaintCaseColor { get; set; }
        public bool IsDomestic { get; set; }
        public bool IsBluetooth { get; set; }
        public string[] Accessories { get; set; }
        public CaseTypes CaseType { get; set; }
        public string Notes { get; set; }
        public string ShopifyOrderUrl { get; set; }
    }

    public enum CaseTypes
    {
        UNKNOWN = -1,
        SLA,
        PETG_PLA,
        DIY,
    }
}
