namespace ImmerDiscordBot.TrelloListener.Contracts.GoogleSheets.Models
{
    /// <summary>  This class represents data in google sheets such as the order number, and the check boxes for if certain things are printed and the shipped out date  </summary>
    public class OrderPrintStatus
    {
        /// <summary>  the order number. eg 4094, 4234, 4333  </summary>
        public string Order { get; set; }
        /// <summary> is true if any of the items have been printed </summary>
        public bool IsKeyboardPrinting { get; set; }

        /// <summary> is true if all of the items have been printed </summary>
        public bool AreAllPartsPrinted { get; set; }

        /// <summary> is true if the shipped box has a value </summary>
        public bool IsShipped { get; set; }
    }
}
