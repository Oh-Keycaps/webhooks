using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if (string.IsNullOrEmpty(requestBody)) return new OkResult();
            var data = JToken.Parse(requestBody);
            var triggerEvent = JsonConvert.DeserializeObject<TrelloObjects.TriggerEvent>(requestBody);
            switch (triggerEvent.Action.Type)
            {
                case TrelloObjects.ActionTypes.AddAttachmentToCard:
                    if (triggerEvent.Action.Data.Attachment.Name.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase))
                        await SendMessageToDiscord(SendAttachment, triggerEvent);
                    break;
                case TrelloObjects.ActionTypes.UpdateCheckItemStateOnCard:
                    await SendMessageToDiscord(SendCheckItemChecked, triggerEvent);
                    break;
                case TrelloObjects.ActionTypes.UpdateCard:
                    if ((data.SelectToken("action.display.translationKey").Value<string>().Equals("action_move_card_from_list_to_list")))
                        await SendMessageToDiscord(SendCardMoved, triggerEvent);
                    break;
            }

            return new OkResult();
        }

        private static Lazy<Dictionary<string, string>> UserStrings = new Lazy<Dictionary<string, string>>(GetUsers);

        private static async Task SendMessageToDiscord(System.Action<DiscordObjects.EmbedObject, TrelloObjects.TriggerEvent> Playground, TrelloObjects.TriggerEvent triggerEvent)
        {
            var action = triggerEvent.Action;
            using var discord = Discord();
            var content = new DiscordObjects.ExecuteWebhook
            {
                Embeds = new List<DiscordObjects.EmbedObject>(),
                AllowedMentions = StandardMentions(),
            };
            var o = new DiscordObjects.EmbedObject
            {
                Title = $"Trello: {triggerEvent.Model.Name} update",
                Fields = new List<DiscordObjects.EmbedFieldObject>(StandardFields(action)),
                Provider = new DiscordObjects.EmbedProviderObject
                {
                    Url = triggerEvent.Model.Url,
                    Name = "Trello"
                },
                Thumbnail = GetMemberCreatorThumbnail(action.MemberCreator)
            };
            if (triggerEvent.Action.Data.Card != null)
            {
                o.Url = $"https://trello.com/c/{triggerEvent.Action.Data.Card.ShortLink}";
            }

            content.Embeds.Add(o);
            Playground(o, triggerEvent);

            await discord.ExecuteWebhook(content);
        }

        private static void SendCheckItemChecked(DiscordObjects.EmbedObject message, TrelloObjects.TriggerEvent triggerEvent)
        {
            var action = triggerEvent.Action;
            var checkItem = action.Data.CheckItem;
            message.Description = $"{action.MemberCreator.FullName} {checkItem.State}d checklist item \"{checkItem.Name}\" in checklist \"{action.Data.CheckList.Name}\"";
        }

        private static void SendCardMoved(DiscordObjects.EmbedObject message, TrelloObjects.TriggerEvent triggerEvent)
        {
            var a = triggerEvent.Action.Data.ListAfter.Name;
            var b = triggerEvent.Action.Data.ListBefore.Name;
            message.Description = $"{triggerEvent.Action.Data.Card.Name} moved from `{b}` to `{a}`";
        }

        private static void SendAttachment(DiscordObjects.EmbedObject message, TrelloObjects.TriggerEvent triggerEvent)
        {
            var action = triggerEvent.Action;
            message.Description = $"{action.MemberCreator.FullName} added an attachment";
            message.Image = new DiscordObjects.EmbedImageObject
            {
                Url = action.Data.Attachment.Url,
            };
        }

        private static DiscordObjects.EmbedFieldObject[] StandardFields(TrelloObjects.TriggerAction action)
        {
            var list = new List<DiscordObjects.EmbedFieldObject>
            {
                new DiscordObjects.EmbedFieldObject
                {
                    IsInline = true,
                    Name = "Member:",
                    Value = $"{action.MemberCreator.FullName} ({action.MemberCreator.UserName})",
                },
                new DiscordObjects.EmbedFieldObject
                {
                    IsInline = true,
                    Name = "Card:",
                    Value = $"[{action.Data.Card.Name}](https://trello.com/c/{action.Data.Card.ShortLink})"
                }
            };
            var dict = UserStrings.Value;
            if (dict.ContainsKey(action.MemberCreator.UserName))
            {
                list.Add(
                    new DiscordObjects.EmbedFieldObject
                    {
                        IsInline = false,
                        Name = "Discord User:",
                        Value = dict[action.MemberCreator.UserName]
                    });
            }

            return list.ToArray();
        }

        private static DiscordObjects.Mentions StandardMentions()
        {
            return new DiscordObjects.Mentions
            {
                MentionTypes = new string[] {DiscordObjects.MentionTypes.Users},
                Users = new object[0]
            };
        }

        private static Dictionary<string, string> GetUsers()
        {
            //TODO: go to https://trello.com/c/4sH956sm and get my comment with my @trelloUser = (?discordUserId<@\d+>) till then
            return new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                {"mrsaspira", "<@671447566990180362>"},
                {"sergeykolbunov", "<@423683303351255050>"},
                {"zera110011", "<@275119482035240960>"},
                {"jaketalley2", "<@352512074653368340>"},
                {"kswestfall1", "<@330138066369118209>"},
                {"nickbornt", "<@211298225699684353>"},
                {"danielgordon69", "<@349350939766947853>"},
                {"robertsnyder20", "<@409018285561085967>"},
                {"matthew16633808", "<@715310990341963836>"},
            };
        }

        private static DiscordObjects.EmbedThumbnailObject GetMemberCreatorThumbnail(TrelloObjects.Member member)
        {
            if (string.IsNullOrEmpty(member.AvatarUrl))
                return null;

            return new DiscordObjects.EmbedThumbnailObject
            {
                Url = member.AvatarUrl,
                Height = 100,
                Width = 100
            };
        }

        private static DiscordWebHook Discord() => new DiscordWebHook(
            System.Environment.GetEnvironmentVariable("Discord:WebhookId", EnvironmentVariableTarget.Process),
            System.Environment.GetEnvironmentVariable("Discord:WebhookToken", EnvironmentVariableTarget.Process)
        );
    }
}