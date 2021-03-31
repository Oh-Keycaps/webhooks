using System;
using System.Collections.Generic;

namespace ImmerDiscordBot.TrelloListener.Core.Trello
{
    public class TrelloUserService
    {
        private readonly Lazy<Dictionary<string, string>> _userStrings;

        public TrelloUserService()
        {
            _userStrings = new Lazy<Dictionary<string, string>>(GetUsers);
        }

        public string GetDiscordUserId(string memberCreatorUserName)
        {
            return _userStrings.Value.ContainsKey(memberCreatorUserName) ?
                _userStrings.Value[memberCreatorUserName] :
                null;
        }

        private Dictionary<string, string> GetUsers()
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
    }
}