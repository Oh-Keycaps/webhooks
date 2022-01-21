using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.Core.Trello.Models
{
    public class TrelloListCards
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("idList")]
        public string ListId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("pos")]
        public float Position { get; set; }
        [JsonProperty("isTemplate")]
        public bool IsTemplate { get; set; }
    }
}
