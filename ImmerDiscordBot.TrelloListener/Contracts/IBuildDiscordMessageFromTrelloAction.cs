using System;
using ImmerDiscordBot.TrelloListener.DiscordObjects;
using ImmerDiscordBot.TrelloListener.TrelloObjects;

namespace ImmerDiscordBot.TrelloListener.Contracts
{
    public interface IBuildDiscordMessageFromTrelloAction
    {
        Action<EmbedObject> Build(TriggerEvent trelloEvent);
    }

    public class NullBuilder : IBuildDiscordMessageFromTrelloAction
    {
        public Action<EmbedObject> Build(TriggerEvent trelloEvent) => _ => { };
    }
    public class CardMovedToNewList : IBuildDiscordMessageFromTrelloAction
    {
        public Action<EmbedObject> Build(TriggerEvent triggerEvent)
        {
            var a = triggerEvent.Action.Data.ListAfter.Name;
            var b = triggerEvent.Action.Data.ListBefore.Name;
            return message =>
            {
                message.Description = $"{triggerEvent.Action.Data.Card.Name} moved from `{b}` to `{a}`";
            };
        }
    }

    public class CardGotNewAttachment : IBuildDiscordMessageFromTrelloAction
    {
        public Action<EmbedObject> Build(TriggerEvent triggerEvent)
        {
            var action = triggerEvent.Action;
            return message =>
            {
                message.Description = $"{action.MemberCreator.FullName} added an attachment";
                message.Image = new EmbedImageObject
                {
                    Url = action.Data.Attachment.Url,
                };
            };
        }
    }

    public class ItemChecked : IBuildDiscordMessageFromTrelloAction
    {
        public Action<EmbedObject> Build(TriggerEvent triggerEvent)
        {
            var action = triggerEvent.Action;
            var checkItem = action.Data.CheckItem;
            return message =>
            {
                message.Description = $"{action.MemberCreator.FullName} {checkItem.State}d checklist item \"{checkItem.Name}\" in checklist \"{action.Data.CheckList.Name}\"";
            };
        }
    }
}
