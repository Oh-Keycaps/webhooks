using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ImmerDiscordBot.TrelloListener.Core
{
    public class TrelloClient : IDisposable
    {
        private readonly ILogger _logger;
        private readonly HttpClient _client;
        private readonly string _trelloAuth;

        public TrelloClient(ILogger<TrelloClient> logger, TrelloClientSettings settings)
        {
            _client = new HttpClient {BaseAddress = new Uri("https://api.trello.com/")};

            _logger = logger;
            _trelloAuth = $"key={settings.Key}&token={settings.Token}";
        }

        public async Task<string> GetCardComments(string cardId, CancellationToken cancellationToken)
        {
            var response = await _client.GetAsync($"1/cards/{cardId}/actions?filter=commentCard&{_trelloAuth}");
            await using var stream = await response.Content.ReadAsStreamAsync();
            using var streamReader = new StreamReader(stream);
            using var reader = new JsonTextReader(streamReader);
            var token = await JToken.LoadAsync(reader, cancellationToken);
            return await GetSuccessfulContent(response);
        }

        internal async Task<T> GetSuccessfulContent<T>(HttpResponseMessage g)
        {
            return JsonConvert.DeserializeObject<T>(await GetSuccessfulContent(g));
        }

        internal async Task<string> GetSuccessfulContent(HttpResponseMessage g)
        {
            var content = await g.Content.ReadAsStringAsync();
            if (!g.IsSuccessStatusCode)
            {
                _logger.LogError(content);
            }

            g.EnsureSuccessStatusCode();
            return content;
        }

        public void Dispose() => _client.Dispose();
    }
}