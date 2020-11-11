using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.TrelloObjects
{
    public class Member
    {
        [JsonProperty("avatarUrl")]
        public string AvatarUrl {get;set;}
        [JsonProperty("fullName")]
        public string FullName {get;set;}
        [JsonProperty("username")]
        public string UserName {get;set;}
    }
}
