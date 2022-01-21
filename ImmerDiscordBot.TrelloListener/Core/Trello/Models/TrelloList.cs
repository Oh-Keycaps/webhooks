using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.Core.Trello.Models
{
    public class TrelloList
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("cards")]
        public TrelloListCards[] Cards { get; set; }
    }
}
