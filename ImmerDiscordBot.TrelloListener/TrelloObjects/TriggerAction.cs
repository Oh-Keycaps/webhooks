using System;
using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.TrelloObjects
{
    public class TriggerAction
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("data")] public TriggerData Data { get; set; }
        [JsonProperty("date")] public DateTime Date { get; set; }
        [JsonProperty("idMemberCreator")] public string idMemberCreator { get; set; }
        [JsonProperty("memberCreator")] public Member MemberCreator { get; set; }
        [JsonProperty("type")] public string Type { get; set; }
    }
}
