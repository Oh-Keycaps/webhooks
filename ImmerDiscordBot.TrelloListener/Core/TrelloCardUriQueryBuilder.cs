namespace ImmerDiscordBot.TrelloListener.Core
{
    public abstract class TrelloCardUriQueryBuilder : TrelloUriQueryBuilder
    {
        public TrelloCardUriQueryBuilder(TrelloClientSettings settings) : base(settings)
        {
            RestApiEndpoint = "/1/cards";
        }
    }
}
