using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.DiscordObjects
{
    public class EmbedFieldObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        /// <summary>if true then sets field objects in same line, but if you have more than 3 objects with enabled inline or just too long you will get rows with 3 fields in each one or with 2 fields if you used</summary>
        [JsonProperty("inline")]
        public bool? IsInline {get;set;}
    }
}
