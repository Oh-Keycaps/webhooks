using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.TrelloObjects
{
    public class CheckItem
    {
        [JsonProperty("id")]
        public string Id {get;set;}
        [JsonProperty("name")]
        public string Name {get;set;}
        [JsonProperty("state")]
        public string State {get;set;}
    }
}
