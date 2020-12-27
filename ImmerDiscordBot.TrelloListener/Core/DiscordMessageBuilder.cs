using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.Contracts;
using ImmerDiscordBot.TrelloListener.DiscordObjects;
using ImmerDiscordBot.TrelloListener.TrelloObjects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ImmerDiscordBot.TrelloListener.Core
{
    public class DiscordMessageBuilder
    {
        private readonly DiscordWebHook _discordWebHook;
        private readonly TrelloUserService _trelloUserService;
        private readonly ILogger _logger;

        public DiscordMessageBuilder(DiscordWebHook discordWebHook, TrelloUserService trelloUserService, ILogger<DiscordMessageBuilder> logger)
        {
            _discordWebHook = discordWebHook;
            _trelloUserService = trelloUserService;
            _logger = logger;
        }

        public async Task SendMessageToDiscord(JToken data)
        {
            var triggerEvent = data.ToObject<TriggerEvent>();

            switch (triggerEvent.Action.Type)
            {
                case ActionTypes.AddAttachmentToCard:
                    if(triggerEvent.AttachmentHasExtensionType(".jpeg") || triggerEvent.AttachmentHasExtensionType(".jpg"))
                        await SendMessageToDiscord(new CardGotNewAttachment(), triggerEvent);
                    break;
                case ActionTypes.UpdateCheckItemStateOnCard:
                    await SendMessageToDiscord(new ItemChecked(), triggerEvent);
                    break;
                case ActionTypes.UpdateCard:
                    if ((data.SelectToken("action.display.translationKey").Value<string>().Equals("action_move_card_from_list_to_list")))
                        await SendMessageToDiscord(new CardMovedToNewList(), triggerEvent);
                    break;
            }

        }
        private async Task SendMessageToDiscord(IBuildDiscordMessageFromTrelloAction playground, TriggerEvent triggerEvent)
        {
            var action = triggerEvent.Action;
            var content = new ExecuteWebhook
            {
                Embeds = new List<EmbedObject>(),
                AllowedMentions = StandardMentions(),
            };
            var embedObject = new EmbedObject
            {
                Title = $"Trello: {triggerEvent.Model.Name} update",
                Fields = new List<EmbedFieldObject>(StandardFields(action)),
                Provider = new EmbedProviderObject
                {
                    Url = triggerEvent.Model.Url,
                    Name = "Trello"
                },
                Thumbnail = GetMemberCreatorThumbnail(action.MemberCreator)
            };
            if (triggerEvent.Action.Data.Card != null)
            {
                embedObject.Url = $"https://trello.com/c/{triggerEvent.Action.Data.Card.ShortLink}";
            }

            content.Embeds.Add(embedObject);
            var builder = playground.Build(triggerEvent);
            builder(embedObject);

            await _discordWebHook.ExecuteWebhook(content);
        }

        private EmbedFieldObject[] StandardFields(TriggerAction action)
        {
            var list = new List<EmbedFieldObject>
            {
                new EmbedFieldObject
                {
                    IsInline = true,
                    Name = "Member:",
                    Value = $"{action.MemberCreator.FullName} ({action.MemberCreator.UserName})",
                },
                new EmbedFieldObject
                {
                    IsInline = true,
                    Name = "Card:",
                    Value = $"[{action.Data.Card.Name}](https://trello.com/c/{action.Data.Card.ShortLink})"
                }
            };
            var discordUserId = _trelloUserService.GetDiscordUserId(action.MemberCreator.UserName);

            if (!string.IsNullOrEmpty(discordUserId))
            {
                list.Add(
                    new EmbedFieldObject
                    {
                        IsInline = false,
                        Name = "Discord User:",
                        Value = discordUserId
                    });
            }

            return list.ToArray();
        }

        private static Mentions StandardMentions()
        {
            return new Mentions
            {
                MentionTypes = new[] {MentionTypes.Users},
                Users = new object[0]
            };
        }

        private static EmbedThumbnailObject GetMemberCreatorThumbnail(Member member)
        {
            if (string.IsNullOrEmpty(member.AvatarUrl))
                return null;

            return new EmbedThumbnailObject
            {
                Url = member.AvatarUrl,
                Height = 100,
                Width = 100
            };
        }
    }
}
