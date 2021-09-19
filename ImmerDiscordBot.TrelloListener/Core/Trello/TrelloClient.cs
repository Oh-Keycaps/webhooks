using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ImmerDiscordBot.TrelloListener.Core.Trello
{
    public class TrelloClient : IDisposable
    {
        private readonly ILogger _logger;
        private readonly TrelloClientSettings _settings;
        private readonly HttpClient _client;

        public TrelloClient(ILogger<TrelloClient> logger, IOptions<TrelloClientSettings> settings)
        {
            _client = new HttpClient {BaseAddress = new Uri("https://api.trello.com/")};

            _logger = logger;
            _settings = settings.Value;
        }

        public async Task<string> CreateCard(TrelloCardToCreate card, CancellationToken token)
        {
            const string dactylsToPrintList = "5db1f059f4e70129456ec11f";
            const string orderCardTemplateId = "5db2e495e40a6877cd888aac";
            const string bluetoothLabelId = "5f286a5fe0ec6f2206e6adbf";
            const string hotswapLabelId = "5db1f0598bdee58e0d989d53";
            const string diyOrderLabelId = "5db1f0598bdee58e0d989d5b";
            var sb = new StringBuilder()
                .AppendLine($"{card.CaseColor} {card.CaseVariant}")
                .AppendLine();
            if(string.IsNullOrEmpty(card.LEDs))
                sb.AppendLine("- [Switches] None");
            else
                sb.AppendLine($"- [Switches] {card.Switches}");

            sb
                .AppendLine($"- [Wrist rest color] {card.WristRestColor}")
                .AppendLine($"- [MCU] {card.MCU}");
            if(string.IsNullOrEmpty(card.LEDs))
                sb.AppendLine("- [LEDs?] None");
            else
                sb.AppendLine($"- [LEDs?] {card.LEDs}");

            var country = card.IsDomestic ? "Domestic" : "International";
            sb.AppendLine($"- [Domestic/International] {country}");
            sb.AppendLine("- [Accessories (keycaps, cords, etc)]");
            foreach (var acc in card.Accessories)
                sb.AppendLine($"  * {acc}");
            var uri = new TrelloCreateCardUriQueryBuilder(_settings, dactylsToPrintList)
            {
                Name = "Order " + card.OrderName,
                IdCardSource = orderCardTemplateId,
                KeepFromSource = "checklists",
                SetPositionBottom = true,
                Description = sb.ToString(),
            };
            var labels = new List<string>();
            if (card.IsBluetooth)
                labels.Add(bluetoothLabelId);
            if (card.CaseType == CaseTypes.DIY)
                labels.Add(diyOrderLabelId);
            if (card.CaseType == CaseTypes.SLA)
                labels.Add(hotswapLabelId);

            if (labels.Any())
            {
                uri.Labels = string.Join(",", labels);
            }

            var response = await _client.PostAsync(uri.ToUriString(), null, token);
            await using var stream = await response.Content.ReadAsStreamAsync();
            using var streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd();
        }

        public async Task<string> GetCardComments(string cardId, CancellationToken cancellationToken)
        {
            var uri = new TrelloGetCardUriQueryBuilder(_settings, cardId)
            {
                Filter = "commentCard"
            };
            var response = await _client.GetAsync(uri.ToUriString(), cancellationToken);
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
