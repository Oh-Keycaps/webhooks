using System;
using ImmerDiscordBot.TrelloListener.TrelloObjects;

namespace ImmerDiscordBot.TrelloListener.Core
{
    public static class TrelloAttachmentExtensions
    {
        public static bool AttachmentHasExtensionType(this TriggerEvent triggerEvent, string extension) => triggerEvent.Action.Data.Attachment.IsExtensionType(extension);
        public static bool IsExtensionType(this Attachment attachment, string extension) => attachment.Name.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase);
    }
}
