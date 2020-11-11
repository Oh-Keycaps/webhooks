using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.DiscordObjects
{
    public class EmbedObject
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; } = "rich";
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("timestamp")]
        public string TimeStamp => DateTime.UtcNow.ToString("O");
        [JsonProperty("color")]
        public int? Color { get; set; }
        [JsonProperty("footer")]
        public EmbedFooterObject Footer { get; set; }
        [JsonProperty("image")]
        public EmbedImageObject Image {get;set;}
        [JsonProperty("thumbnail")]
        public EmbedThumbnailObject Thumbnail {get;set;}
        [JsonProperty("provider")]
        public EmbedProviderObject Provider {get;set;}
        [JsonProperty("fields")]
        public List<EmbedFieldObject> Fields { get; set; }
    }
}
