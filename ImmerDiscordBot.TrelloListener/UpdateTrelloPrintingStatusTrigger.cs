using System;
using System.Threading.Tasks;
using System.Web.Http;
using ImmerDiscordBot.TrelloListener.Core.Trello;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace ImmerDiscordBot.TrelloListener
{
    public class UpdateTrelloPrintingStatusTrigger
    {
        private readonly TrelloUpdateOrderPrintStatus _updateOrderPrintStatus;

        public UpdateTrelloPrintingStatusTrigger(TrelloUpdateOrderPrintStatus updateOrderPrintStatus)
        {
            _updateOrderPrintStatus = updateOrderPrintStatus;
        }

        /// <summary>
        /// Run everyday at 1am
        /// </summary>
        /// <param name="myTimer">The time the trigger was ran</param>
        /// <param name="log"></param>
        [Disable("DisableUpdateTrelloPrintingStatusTrigger"), FunctionName("UpdateTrelloPrintingStatusTrigger")]
        public Task RunAsync([TimerTrigger("0 0 1 * * *")] TimerInfo myTimer, ILogger log)
        {
            return Execute(log);
        }

        /// <summary>
        /// Run everyday at 1am
        /// </summary>
        /// <param name="myTimer">The time the trigger was ran</param>
        /// <param name="log"></param>
        [Disable("DisableUpdateTrelloPrintingStatusTrigger"), FunctionName("ManuallyUpdateTrelloPrintingStatusTrigger")]
        public async Task<IActionResult> ManuallyRunAsync([HttpTrigger(AuthorizationLevel.Function, "post")]HttpRequest req, ILogger log)
        {
            try
            {
                await Execute(log);
                return new OkResult();
            }
            catch (Exception e)
            {
                return new ObjectResult(e){StatusCode = StatusCodes.Status500InternalServerError};
            }
        }

        private async Task Execute(ILogger log)
        {
            log.LogInformation("Executing Azure Function Update Trello Printing Status");
            await _updateOrderPrintStatus.UpdateOrderPrintStatus("45yAlV3N", log);
            log.LogInformation("Executed Azure Function Update Trello Printing Status");
        }
    }
}
