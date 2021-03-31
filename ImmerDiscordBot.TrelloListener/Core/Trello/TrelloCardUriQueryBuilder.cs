namespace ImmerDiscordBot.TrelloListener.Core.Trello
{
    public abstract class TrelloCardUriQueryBuilder : TrelloUriQueryBuilder
    {
        public TrelloCardUriQueryBuilder(TrelloClientSettings settings) : base(settings)
        {
            RestApiEndpoint = "/1/cards";
        }
    }
}
