using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using ImmerDiscordBot.TrelloListener.Core.Shopify.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImmerDiscordBot.TrelloListener.Core.GoogleSheets
{
    public class SheetsClient
    {
        private readonly SheetsServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private readonly GoogleSheetsSettings _settings;

        public SheetsClient(SheetsServiceProvider provider, IOptions<GoogleSheetsSettings> options, ILogger<SheetsClient> logger)
        {
            _serviceProvider = provider;
            _logger = logger;
            _settings = options.Value;
        }

        public Task Append(TrelloCardToCreate data, CancellationToken token)
        {
            _logger.LogDebug("Sending order {0} to {1}!{2}", data.OrderName, _settings.DocumentId, _settings.SheetId);
            var service = _serviceProvider.Get();
            var range = $"{_settings.SheetId}!A1";

            var body = new ValueRange
            {
                Values = new List<IList<object>>{CreateRow(data)},
            };
            var request = service.Spreadsheets.Values.Append(body, _settings.DocumentId, range);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
            request.IncludeValuesInResponse = true;

            return request.ExecuteAsync(token);
        }

        private static object[] CreateRow(TrelloCardToCreate data)
        {
            return new object[]
            {
                DateTime.UtcNow.ToString("MM\\/dd\\/yyyy"), //A
                data.OrderName.Trim('#'), //B
                data.CaseVariant, //C
                data.CaseColor, //D
                data.MCU, //E
                data.CaseType.ToString(),
            };
        }
    }
}
