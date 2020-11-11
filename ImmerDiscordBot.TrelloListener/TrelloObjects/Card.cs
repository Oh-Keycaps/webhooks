using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.TrelloObjects
{
    public class Card
    {
        [JsonProperty("id")]
        public string Id {get;set;}
        [JsonProperty("idShort")]
        public int IdShort {get;set;}
        [JsonProperty("name")]
        public string Name {get;set;}
        [JsonProperty("shortLink")]
        public string ShortLink {get;set;}
    }
}
