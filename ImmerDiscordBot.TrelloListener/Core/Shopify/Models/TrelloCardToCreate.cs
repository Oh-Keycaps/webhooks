namespace ImmerDiscordBot.TrelloListener.Core.Shopify.Models
{
    public struct TrelloCardToCreate
    {
        public string OrderName;
        public string CaseColor;
        public string CaseVariant;
        public string Switches;
        public string WristRestColor;
        public string MCU;
        public string LEDs;
        public string PaintCaseColor;
        public bool IsDomestic;
        public bool IsBluetooth;
        public string[] Accessories;
        public CaseTypes CaseType;
    }

    public enum CaseTypes
    {
        UNKNOWN = -1,
        SLA,
        PETG_PLA,
    }
}
