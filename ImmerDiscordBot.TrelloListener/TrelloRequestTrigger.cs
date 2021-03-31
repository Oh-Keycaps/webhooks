using System.IO;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.Core;
using ImmerDiscordBot.TrelloListener.Core.Discord;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ImmerDiscordBot.TrelloListener
{
    public class TrelloRequestTrigger
    {
        private readonly DiscordMessageBuilder _discord;

        public TrelloRequestTrigger(DiscordMessageBuilder discord)
        {
            _discord = discord;
        }

        [FunctionName("TrelloRequestTrigger")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post", "head")]
            HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody)) return new OkResult();

            await _discord.SendMessageToDiscord(JToken.Parse(requestBody));

            return new OkResult();
        }
    }
}
