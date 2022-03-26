using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Http;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImmerDiscordBot.TrelloListener.Core.GoogleSheets
{
    public class SheetsClient : IHttpUnsuccessfulResponseHandler
    {
        private bool _requestIsUnsuccessful;
        private string _responseContent;

        private readonly SheetsServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private readonly GoogleSheetsSettings _settings;

        public SheetsClient(SheetsServiceProvider provider, IOptions<GoogleSheetsSettings> options, ILogger<SheetsClient> logger)
        {
            _serviceProvider = provider;
            _logger = logger;
            _settings = options.Value;
        }

        public async Task Append(SheetRow data, CancellationToken token, ILogger logger)
        {
            logger.LogDebug("Sending order {OrderName} to {DocumentId}!{SheetId}", data.OrderName, _settings.DocumentId, _settings.SheetId);
            var service = _serviceProvider.Get();
            var range = $"{_settings.SheetId}!A1";

            var body = new ValueRange
            {
                Values = new List<IList<object>>{CreateRow(data)},
            };
            var request = service.Spreadsheets.Values.Append(body, _settings.DocumentId, range);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            request.IncludeValuesInResponse = true;
            request.AddExceptionHandler(new BackOffHandler(new ExponentialBackOff()));
            request.AddUnsuccessfulResponseHandler(this);

            var response = await request.ExecuteAsync(token);
            using (logger.BeginScope(new { response.TableRange, response.Updates.UpdatedRange, response.Updates.UpdatedRows }))
            {
                if (response.TableRange == response.Updates.UpdatedRange || response.Updates.UpdatedRows.GetValueOrDefault(0) != 1)
                {
                    var exception = new Exception("No rows were updated when appending row to GoogleSheets");
                    logger.LogError(exception, "Range to Update is same as Updated range (meaning no rows updated) or UpdatedRows was not 1");
                    throw exception;
                }

                if (_requestIsUnsuccessful)
                {
                    var exception = new Exception("Something went wrong. Response code is unsuccessful");
                    logger.LogError(exception, "When handling a request to update a row in google sheets the response was unsuccessful. {Content}", _responseContent);
                    throw exception;
                }
                logger.LogInformation("successfully updated GoogleSheets row");
            }
        }

        private static object[] CreateRow(SheetRow data)
        {
            return new object[]
            {
                DateTime.UtcNow.ToString("MM\\/dd\\/yyyy"), //A
                data.OrderName, //B
                data.CaseVariant, //C
                data.CaseColor, //D
                data.MCU, //E
                data.CaseType,
                data.WristRestsIncluded,
                data.Notes,

                $"=HYPERLINK(\"{data.ShopifyOrderUrl}\", \"Shopify Order Link\")",
            };
        }


        public async Task<bool> HandleResponseAsync(HandleUnsuccessfulResponseArgs args)
        {
            _responseContent = await args.Response.Content.ReadAsStringAsync();
            _requestIsUnsuccessful = true;
            _logger.LogWarning("When handling a request to update a row in google sheets the response was unsuccessful. {ResponseCode} {ReasonPhrase} {Content}", args.Response.StatusCode, args.Response.ReasonPhrase, _responseContent);
            return false;
        }
    }
}
