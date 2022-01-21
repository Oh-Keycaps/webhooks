using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;
using ImmerDiscordBot.TrelloListener.Core.Trello.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ImmerDiscordBot.TrelloListener.Core.Trello
{
    public class TrelloClient
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TrelloClientSettings _settings;

        public TrelloClient(ILogger<TrelloClient> logger, IOptions<TrelloClientSettings> settings, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
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
            if(string.IsNullOrEmpty(card.Switches))
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

            sb.AppendLine().AppendLine($"[Shopify Order Link]({card.ShopifyOrderUrl})");
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

            using var httpClient = _httpClientFactory.CreateClient(nameof(TrelloClient));
            var response = await httpClient.PostAsync(uri.ToUriString(), null, token);
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
            using var httpClient = _httpClientFactory.CreateClient(nameof(TrelloClient));
            var response = await httpClient.GetAsync(uri.ToUriString(), cancellationToken);
            await using var stream = await response.Content.ReadAsStreamAsync();
            using var streamReader = new StreamReader(stream);
            using var reader = new JsonTextReader(streamReader);
            var token = await JToken.LoadAsync(reader, cancellationToken);
            return await GetSuccessfulContent(response);
        }

        public async Task<IReadOnlyList<TrelloList>> GetListsOnBoard(string boardId)
        {
            using var httpClient = _httpClientFactory.CreateClient(nameof(TrelloClient));
            var uriBuilder = new TrelloListCardUriQueryBuilder(_settings, boardId)
            {
                Cards = TrelloListCardUriQueryBuilder.ListCards.open,
                Filter = TrelloListCardUriQueryBuilder.ListFilter.all,
                CardFields = "id,idList,pos,name,isTemplate",
                Fields = "id,name"
            };
            var response = await httpClient.GetAsync(uriBuilder.ToUriString());

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var trelloList = JsonConvert.DeserializeObject<TrelloList[]>(content);
            return trelloList.ToImmutableArray();
        }
        internal async Task<T> GetSuccessfulContent<T>(HttpResponseMessage g)
        {
            return JsonConvert.DeserializeObject<T>(await GetSuccessfulContent(g));
        }

        public async Task MoveCardToList(string cardId, string idList)
        {
            var uriBuilder = new TrelloMoveCardToListQueryBuilder(_settings, cardId) { ListId = idList };
            using var httpClient = _httpClientFactory.CreateClient(nameof(TrelloClient));
            var response = await httpClient.PutAsync(uriBuilder.ToUriString(), null);
            response.EnsureSuccessStatusCode();
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
    }
}
