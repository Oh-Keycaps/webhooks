using System.Collections.Generic;
using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.DiscordObjects
{
    public class ExecuteWebhook
    {
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        [JsonProperty("tts")]
        public bool TTS { get; set; }
        [JsonProperty("file")]
        public string File { get; set; }
        [JsonProperty("embeds")]
        public List<EmbedObject> Embeds { get; set; }
        [JsonProperty("payload_json")]
        public string PayloadJson { get; set; }
        [JsonProperty("allowed_mentions")]
        public Mentions AllowedMentions { get; set; }
    }
}
