using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;

namespace ImmerDiscordBot.TrelloListener
{
    public class ShopifyOrderCreateTrigger
    {
        [FunctionName("ShopifyOrderCreateTrigger")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [ServiceBus("startshopify", EntityType = EntityType.Queue)]IAsyncCollector<string> messageCollector,
            ILogger log)
        {
            using var streamReader = new StreamReader(req.Body);
            var requestBody = await streamReader.ReadToEndAsync();
            await messageCollector.AddAsync(requestBody);
            return new OkResult();
        }
    }
}