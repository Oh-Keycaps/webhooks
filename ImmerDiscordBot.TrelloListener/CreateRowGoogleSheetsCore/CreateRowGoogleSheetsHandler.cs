using System.Threading;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using ImmerDiscordBot.TrelloListener.Core.GoogleSheets;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ImmerDiscordBot.TrelloListener.CreateRowGoogleSheetsCore
{
    public class CreateRowGoogleSheetsHandler
    {
        private readonly OrderToSheetRowMapper _orderMapper;
        private readonly SheetsClient _sheetsClient;

        public CreateRowGoogleSheetsHandler(OrderToSheetRowMapper orderMapper, SheetsClient sheetsClient)
        {
            _orderMapper = orderMapper;
            _sheetsClient = sheetsClient;
        }

        [Disable("DisableCreateRowGoogleSheetsFunction"),FunctionName("CreateRowGoogleSheets")]
        public async Task HandleMessage([ServiceBusTrigger("createrowgooglesheets")] Order order, ILogger log, CancellationToken token)
        {
            var sheetRow = _orderMapper.MapToSheetRow(order);
            log.LogDebug("+CreateRowInGoogleSpreadsheets {OrderName}", sheetRow.OrderName);
            await _sheetsClient.Append(sheetRow, token, log);
            log.LogDebug("-CreateRowInGoogleSpreadsheets {OrderName}", sheetRow.OrderName);
        }
    }
}
