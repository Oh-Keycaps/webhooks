using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ImmerDiscordBot.TrelloListener.Contracts.GoogleSheets.Models;
using Microsoft.Extensions.Options;

namespace ImmerDiscordBot.TrelloListener.Core.GoogleSheets
{
    public class OrderPrintStatusProvider
    {
        private readonly GoogleSheetsSettings _settings;
        private readonly SheetsServiceProvider _sheetsServiceProvider;

        public OrderPrintStatusProvider(SheetsServiceProvider sheetsServiceProvider, IOptions<GoogleSheetsSettings> settings)
        {
            _sheetsServiceProvider = sheetsServiceProvider;
            _settings = settings.Value;
        }

        public IReadOnlyList<OrderPrintStatus> GetOrderPrintStatuses()
        {
            var sheetsService = _sheetsServiceProvider.Get();
            var petgValues = sheetsService.Spreadsheets.Values.Get(_settings.DocumentId, "PETG/PLA Cases!B3:K").Execute();
            var slaValues = sheetsService.Spreadsheets.Values.Get(_settings.DocumentId, "SLA Cases!B3:K").Execute();
            return petgValues.Values
                .Concat(slaValues.Values)
                .Select(x => new GoogleSheetsPetgPrintedSheetRowParser(x))
                .Select(x => x.ToOrderPrintStatus())
                .ToImmutableArray();
        }
    }
}
