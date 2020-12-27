namespace ImmerDiscordBot.TrelloListener.TrelloObjects
{
    /// <summary>
    /// https://developer.atlassian.com/cloud/trello/guides/rest-api/action-types/
    /// https://docs.google.com/spreadsheets/d/1opvJZ2yqfWgVr5ol1NXkvhn7Y4RKon2xPhemMbQprwA/edit#gid=0
    /// </summary>
    public static class ActionTypes
    {
        /// <summary></summary>
        public const string AddAttachmentToCard = "addAttachmentToCard";
        /// <summary></summary>
        public const string UpdateCheckItemStateOnCard = "updateCheckItemStateOnCard";
        /// <summary>Used when a card is moved among other things</summary>
        public const string UpdateCard = "updateCard";
    }
}
