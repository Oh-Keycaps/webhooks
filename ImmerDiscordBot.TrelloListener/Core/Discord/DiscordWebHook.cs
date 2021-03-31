using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.DiscordObjects;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.Core.Discord
{
    public class DiscordWebHook : IDisposable
    {
        private readonly HttpClient _client;
        private readonly DiscordSettings _settings;

        public DiscordWebHook(IOptions<DiscordSettings> discordOptions)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://discordapp.com/api/webhooks/")
            };
            _settings = discordOptions.Value;
        }

        public void Dispose() => _client.Dispose();

        public async Task<string> ExecuteWebhook(ExecuteWebhook hook)
        {
            var payload = JsonConvert.SerializeObject(hook, new JsonSerializerSettings {DefaultValueHandling = DefaultValueHandling.Ignore});
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{_settings.WebhookId}/{_settings.WebhookToken}", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }
    }
}