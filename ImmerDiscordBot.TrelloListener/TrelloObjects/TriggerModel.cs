using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.TrelloObjects
{
    public class TriggerModel
    {
        [JsonProperty("name")]
        public string Name {get;set;}
        [JsonProperty("url")]
        public string Url {get;set;}
    }
}
