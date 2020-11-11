using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.DiscordObjects
{
    public class EmbedProviderObject
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
