using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.TrelloObjects
{
    public class Board
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("desc")]
        public string Description { get; set; }
        [JsonProperty("descData")]
        public string DescriptionData { get; set; }
        [JsonProperty("closed")]
        public bool? IsClosed { get; set; }
        [JsonProperty("idOrganization")]
        public string IdOrganization { get; set; }
        [JsonProperty("pinned")]
        public bool? Pinned {get;set;}
        [JsonProperty("url")]
        public string Url {get;set;}
        [JsonProperty("shortUrl")]
        public string ShortUrl {get;set;}
        [JsonProperty("prefs")]
        public object Preferences {get;set;}
        [JsonProperty("labelNames")]
        public object LabelNames { get; set; }
        [JsonProperty("starred")]
        public bool? IsStarred { get; set;}
        [JsonProperty("limits")]
        public object Limits {get;set;}
        [JsonProperty("memberships")]
        public object[] Memberships {get;set;}
        [JsonProperty("enterpriseOwned")]
        public bool? IsEnterpriseOwned {get;set;}
    }
}
