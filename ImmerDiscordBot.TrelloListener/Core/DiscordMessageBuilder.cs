using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.Contracts;
using ImmerDiscordBot.TrelloListener.DiscordObjects;
using ImmerDiscordBot.TrelloListener.TrelloObjects;
using Newtonsoft.Json.Linq;

namespace ImmerDiscordBot.TrelloListener.Core
{
    public class DiscordMessageBuilder
    {
        private readonly DiscordWebHook _discordWebHook;

        public DiscordMessageBuilder(DiscordWebHook discordWebHook)
        {
            _discordWebHook = discordWebHook;
        }


        private static Lazy<Dictionary<string, string>> UserStrings = new Lazy<Dictionary<string, string>>(GetUsers);

        public async Task SendMessageToDiscord(JToken data)
        {
            var triggerEvent = data.ToObject<TriggerEvent>();

            switch (triggerEvent.Action.Type)
            {
                case ActionTypes.AddAttachmentToCard:
                    if (triggerEvent.Action.Data.Attachment.Name.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase))
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

        private static EmbedFieldObject[] StandardFields(TriggerAction action)
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
            var dict = UserStrings.Value;
            if (dict.ContainsKey(action.MemberCreator.UserName))
            {
                list.Add(
                    new EmbedFieldObject
                    {
                        IsInline = false,
                        Name = "Discord User:",
                        Value = dict[action.MemberCreator.UserName]
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
