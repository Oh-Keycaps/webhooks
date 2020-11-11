using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.DiscordObjects;
using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.Core
{
    public class DiscordWebHook : IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _webhookId;
        private readonly string _webhookToken;

        public DiscordWebHook(string webhookId, string webhookToken)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri($"https://discordapp.com/api/webhooks/")
            };
            _webhookId = webhookId;
            _webhookToken = webhookToken;
        }

        public void Dispose() => _client.Dispose();

        public async Task<string> ExecuteWebhook(ExecuteWebhook hook)
        {
            var payload = JsonConvert.SerializeObject(hook, new JsonSerializerSettings {DefaultValueHandling = DefaultValueHandling.Ignore});
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{_webhookId}/{_webhookToken}", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }
    }
}