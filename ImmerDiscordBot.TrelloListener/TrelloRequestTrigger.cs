﻿using System;
using System.IO;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ImmerDiscordBot.TrelloListener
{
    public static class TrelloRequestTrigger
    {
        [FunctionName("TrelloRequestTrigger")]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post", "head")]
            HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody)) return new OkResult();
            using var discordWebHook = Discord();
            var discordMessageBuilder = new DiscordMessageBuilder(discordWebHook);
            await discordMessageBuilder.SendMessageToDiscord(JToken.Parse(requestBody));

            return new OkResult();
        }

        private static DiscordWebHook Discord() => new DiscordWebHook(
            System.Environment.GetEnvironmentVariable("Discord:WebhookId", EnvironmentVariableTarget.Process),
            System.Environment.GetEnvironmentVariable("Discord:WebhookToken", EnvironmentVariableTarget.Process)
        );
    }
}