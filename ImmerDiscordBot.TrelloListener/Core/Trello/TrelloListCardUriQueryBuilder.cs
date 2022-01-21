namespace ImmerDiscordBot.TrelloListener.Core.Trello
{
    public class TrelloListCardUriQueryBuilder : TrelloUriQueryBuilder
    {
        public TrelloListCardUriQueryBuilder(TrelloClientSettings settings, string boardId) : base(settings)
        {
            RestApiEndpoint = $"/1/boards/{boardId}/lists";
        }

        public ListCards Cards { set { _d["cards"] = value.ToString(); } }

        /// <summary> all or comma-separated list of card fields </summary>
        public string CardFields { set { _d["card_fields"] = value; } }

        public ListFilter Filter { set { _d["filter"] = value.ToString(); } }

        /// <summary> all or comma-separated list of card fields </summary>
        public string Fields { set { _d["fields"] = value; } }

        public enum ListCards
        {
            all, closed, none, open
        }

        public enum ListFilter
        {
            all, closed, none, open
        }
    }
}
