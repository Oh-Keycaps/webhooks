namespace ImmerDiscordBot.TrelloListener.Core.Trello
{
    public class TrelloMoveCardToListQueryBuilder : TrelloUriQueryBuilder
    {
        public TrelloMoveCardToListQueryBuilder(TrelloClientSettings settings, string cardId) : base(settings)
        {
            RestApiEndpoint = $"/1/cards/{cardId}";
        }

        /// <summary> The ID of the list the card should be in </summary>
        public string ListId { set { _d["idList"] = value; } }
    }
}
