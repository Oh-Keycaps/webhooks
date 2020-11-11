namespace ImmerDiscordBot.TrelloListener.TrelloObjects
{
    public class TriggerData
    {
        public Attachment Attachment{get;set;}
        public Board Board {get;set;}
        public Card Card {get;set;}
        public CheckItem CheckItem {get;set;}
        public CheckList CheckList {get;set;}
        public TrelloList ListAfter {get;set;}
        public TrelloList ListBefore {get;set;}
    }
}
