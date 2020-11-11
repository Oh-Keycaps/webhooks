using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.TrelloObjects
{
    public class TriggerEvent
    {
        [JsonProperty("action")]
        public TriggerAction Action {get;set;}
        [JsonProperty("model")]
        public TriggerModel Model {get;set;}
    }
}
