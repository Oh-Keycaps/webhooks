namespace ImmerDiscordBot.TrelloListener.Core
{
    public class TrelloGetCardUriQueryBuilder : TrelloUriQueryBuilder
    {
        public string Filter { set => _d["filter"] = value; }

        public TrelloGetCardUriQueryBuilder(TrelloClientSettings settings, string cardId) : base(settings)
        {
            RestApiEndpoint = $"/1/cards/{cardId}/actions";
        }
    }
}
