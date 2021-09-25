using NUnit.Framework;

namespace ImmerDiscordBot.TrelloListener.Core.GoogleSheets
{
    internal class SheetRowTestCaseData : TestCaseData
    {
        public SheetRowTestCaseData(SheetRow expected)
            : base($"data/order-{expected.OrderName}.json", expected)
        {
            SetName($"Mapping Known Order #{expected.OrderName}");
        }
    }
}
