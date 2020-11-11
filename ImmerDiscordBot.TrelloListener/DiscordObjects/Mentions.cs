using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.DiscordObjects
{
    public class Mentions
    {
        [JsonProperty("parse")]
        public string[] MentionTypes {get;set;}
        [JsonProperty("roles")]
        public object[] Roles {get;set;}
        [JsonProperty("users")]
        public object[] Users {get;set;}
    }
}
