using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.TrelloObjects
{
    public class CheckList
    {
        [JsonProperty("id")]
        public string Id {get;set;}
        [JsonProperty("name")]
        public string Name {get;set;}
    }
}
