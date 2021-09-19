using System;
using System.Text.RegularExpressions;

namespace ImmerDiscordBot.TrelloListener.Core.Trello
{
    public class TrelloCreateCardUriQueryBuilder : TrelloCardUriQueryBuilder
    {
        public string Name { set => _d["name"] = value; }
        public string KeepFromSource { set => _d["keepFromSource"] = value; }
        public string Description { set => _d["desc"] = value; }
        public bool SetPositionTop { set => _d["pos"] = "top"; }
        public bool SetPositionBottom { set => _d["pos"] = "bottom"; }
        public uint Order { set => _d["pos"] = value.ToString(); }
        /// <summary> idLabels: Array<string> - Comma-separated list of label IDs to add to the card </summary>
        public string Labels { set => _d["idLabels"]= value; }
        public string IdCardSource
        {
            set
            {
                if (!Regex.IsMatch(value, @"^[0-9a-fA-F]{24}$"))
                    throw new ArgumentException("The id of the card does not match pattern", nameof(IdCardSource));

                _d["idCardSource"] = value;
            }
        }

        public TrelloCreateCardUriQueryBuilder(TrelloClientSettings settings, string listIdToCreateCardIn) : base(settings)
        {
            if(!Regex.IsMatch(listIdToCreateCardIn, @"^[0-9a-fA-F]{24}$"))
                throw new ArgumentException("The id of the list does not match pattern", nameof(listIdToCreateCardIn));

            _d["idList"] = listIdToCreateCardIn;
        }
    }
}
