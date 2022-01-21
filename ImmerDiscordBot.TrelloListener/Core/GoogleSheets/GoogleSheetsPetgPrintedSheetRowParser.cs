using System.Collections.Generic;
using System.Linq;
using ImmerDiscordBot.TrelloListener.Contracts.GoogleSheets.Models;

namespace ImmerDiscordBot.TrelloListener.Core.GoogleSheets
{
    public class GoogleSheetsPetgPrintedSheetRowParser
    {
        private const int OrderColumnIndex = 0;
        /// <summary> this number plus OrderColumnIndex is the index of the first IsPrinted column (as of today and probably forever it is IsTopRight printed) </summary>
        private const int FirstIsPrintedColumnFromOrderOffset = 4;
        private const int SecondIsPrintedColumnFromOrderOffset = FirstIsPrintedColumnFromOrderOffset + 1;
        private const int ThirdIsPrintedColumnFromOrderOffset = SecondIsPrintedColumnFromOrderOffset + 1;
        private const int FourthIsPrintedColumnFromOrderOffset = ThirdIsPrintedColumnFromOrderOffset + 1;
        private const int FifthIsPrintedColumnFromOrderOffset = FourthIsPrintedColumnFromOrderOffset + 1;
        private const int ShippedOutDateColumnFromOrderOffset = FifthIsPrintedColumnFromOrderOffset + 1;
        public string Order => _row[OrderColumnIndex].ToString();
        public bool IsTopRightPrinted => ParseBoolSafe(OrderColumnIndex + FirstIsPrintedColumnFromOrderOffset);
        public bool IsTopLeftPrinted => ParseBoolSafe(OrderColumnIndex + SecondIsPrintedColumnFromOrderOffset);
        public bool IsBottomRightPrinted => ParseBoolSafe(OrderColumnIndex + ThirdIsPrintedColumnFromOrderOffset);
        public bool IsBottomLeftPrinted => ParseBoolSafe(OrderColumnIndex + FourthIsPrintedColumnFromOrderOffset);
        public bool IsWristPrinted => ParseBoolSafe(OrderColumnIndex + FifthIsPrintedColumnFromOrderOffset);

        private bool ParseBoolSafe(int index)
        {
            var value = _row.ElementAtOrDefault(index)?.ToString();
            if (string.IsNullOrWhiteSpace(value)) return false;
            return bool.Parse(value ?? bool.FalseString);
        }

        private readonly IList<object> _row;

        public GoogleSheetsPetgPrintedSheetRowParser(IList<object> row)
        {
            _row = row;
        }

        public OrderPrintStatus ToOrderPrintStatus()
        {
            var shippedOutDate = _row.ElementAtOrDefault(OrderColumnIndex + ShippedOutDateColumnFromOrderOffset)?.ToString() ?? "";
            return new OrderPrintStatus
            {
                Order = Order.Trim(),
                IsShipped = !string.IsNullOrEmpty(shippedOutDate),
                IsKeyboardPrinting = IsTopLeftPrinted || IsTopRightPrinted || IsBottomLeftPrinted || IsBottomRightPrinted || IsWristPrinted,
                AreAllPartsPrinted = IsTopLeftPrinted && IsTopRightPrinted && IsBottomLeftPrinted && IsBottomRightPrinted && IsWristPrinted
            };
        }
    }
}
