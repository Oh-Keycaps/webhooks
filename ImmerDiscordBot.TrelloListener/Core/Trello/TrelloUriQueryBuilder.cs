using System.Collections.Generic;
using System.Linq;

namespace ImmerDiscordBot.TrelloListener.Core.Trello
{
    public abstract class TrelloUriQueryBuilder
    {
        protected string RestApiEndpoint {get; set;}

        protected Dictionary<string, string> _d;
        protected TrelloUriQueryBuilder(TrelloClientSettings settings)
        {
            _d = new Dictionary<string, string>()
            {
                { "key", settings.Key},
                { "token", settings.Token},
            };
        }

        public string ToUriString()
        {
            return $"{RestApiEndpoint}?" + string.Join("&", _d.Select(x => $"{x.Key}={System.Net.WebUtility.UrlEncode(x.Value)}"));
        }
    }
}
