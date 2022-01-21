using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.Contracts.GoogleSheets.Models;
using ImmerDiscordBot.TrelloListener.Core.GoogleSheets;
using ImmerDiscordBot.TrelloListener.Core.Trello.Models;
using Microsoft.Extensions.Logging;

namespace ImmerDiscordBot.TrelloListener.Core.Trello
{
    public class TrelloUpdateOrderPrintStatus
    {
        private readonly OrderPrintStatusProvider _orderPrintStatusProvider;
        private readonly TrelloClient _trelloClient;

        public TrelloUpdateOrderPrintStatus(OrderPrintStatusProvider orderPrintStatusProvider, TrelloClient trelloClient)
        {
            _orderPrintStatusProvider = orderPrintStatusProvider;
            _trelloClient = trelloClient;
        }

        public async Task UpdateOrderPrintStatus(string boardId, ILogger logger)
        {
            using (logger.BeginScope("Updating Order Print Status on board {BoardId}", boardId))
            {
                var printStatuses = _orderPrintStatusProvider.GetOrderPrintStatuses();
                var lists = await _trelloClient.GetListsOnBoard(boardId);
                var dactylsToPrintList = GetListByName(lists, "Dactyls to Print", boardId, logger);
                var printingList = GetListByName(lists, "Printing", boardId, logger);
                var printedList = GetListByName(lists, "Printed", boardId, logger);
                var atDans = GetListByName(lists, "At Dans or Shipped to Dans", boardId, logger);

                await UpdateDactylsToPrintList(boardId, dactylsToPrintList, printStatuses, atDans, printedList, printingList, logger);
                await UpdatePrintingList(boardId, printingList, printStatuses, atDans, printedList, logger);
                await UpdatePrintedList(boardId, printedList, printStatuses, atDans, logger);
            }
        }

        private async Task UpdateDactylsToPrintList(string boardId, TrelloList dactylsToPrintList, IReadOnlyList<OrderPrintStatus> printStatuses, TrelloList atDans, TrelloList printedList, TrelloList printingList, ILogger logger)
        {
            using (logger.BeginScope("Updating {ListName} cards", dactylsToPrintList.Name))
            {
                logger.LogInformation("checking cards in {ListName} to be moved", dactylsToPrintList.Name);
                foreach (var card in dactylsToPrintList.Cards)
                {
                    if (card.IsTemplate) continue;
                    var match = Regex.Match(card.Name, @"^Order.*?(\d+)");
                    if (!match.Success) continue;
                    var orderId = match.Groups[1].Value;
                    var orderPrintStatus = printStatuses.FirstOrDefault(x => x.Order.Contains(orderId, StringComparison.InvariantCultureIgnoreCase));
                    if (orderPrintStatus == null)
                    {
                        logger.LogError(new Exception($"Card {card.Name} is in {dactylsToPrintList.Name} but was not found in print queue google sheet"), "Card {CardName} is in {ListName} but was not found in print queue google sheet", card.Name,
                            dactylsToPrintList.Name);
                        continue;
                    }

                    if (orderPrintStatus.IsShipped)
                    {
                        await MoveCardToList(boardId, card, atDans, logger);
                    }
                    else if (orderPrintStatus.AreAllPartsPrinted)
                    {
                        await MoveCardToList(boardId, card, printedList, logger);
                    }
                    else if (orderPrintStatus.IsKeyboardPrinting)
                    {
                        await MoveCardToList(boardId, card, printingList, logger);
                    }
                }
                logger.LogInformation("checked cards in {ListName} to be moved", dactylsToPrintList.Name);
            }
        }

        private async Task UpdatePrintingList(string boardId, TrelloList printingList, IReadOnlyList<OrderPrintStatus> printStatuses, TrelloList atDans, TrelloList printedList, ILogger logger)
        {
            using (logger.BeginScope("Updating {ListName} cards", printingList.Name))
            {
                logger.LogInformation("checking cards in {ListName} to be moved", printingList.Name);
                foreach (var card in printingList.Cards)
                {
                    if (card.IsTemplate) continue;
                    var match = Regex.Match(card.Name, @"^Order.*?(\d+)");
                    if (!match.Success) continue;
                    var orderId = match.Groups[1].Value;
                    var orderPrintStatus = printStatuses.FirstOrDefault(x => x.Order.Contains(orderId, StringComparison.InvariantCultureIgnoreCase));
                    if (orderPrintStatus == null)
                    {
                        logger.LogError(new Exception($"Card {card.Name} is in {printingList.Name} but was not found in print queue google sheet"), "Card {CardName} is in {ListName} but was not found in print queue google sheet", card.Name,
                            printingList.Name);
                        continue;
                    }

                    if (orderPrintStatus.IsShipped)
                    {
                        await MoveCardToList(boardId, card, atDans, logger);
                    }
                    else if (orderPrintStatus.AreAllPartsPrinted)
                    {
                        await MoveCardToList(boardId, card, printedList, logger);
                    }
                }
                logger.LogInformation("checked cards in {ListName} to be moved", printingList.Name);
            }
        }

        private async Task UpdatePrintedList(string boardId, TrelloList printedList, IReadOnlyList<OrderPrintStatus> printStatuses, TrelloList atDans, ILogger logger)
        {
            using (logger.BeginScope("Updating {ListName} cards", printedList.Name))
            {
                logger.LogInformation("checking cards in {ListName} to be moved", printedList.Name);
                foreach (var card in printedList.Cards)
                {
                    if (card.IsTemplate) continue;
                    var match = Regex.Match(card.Name, @"^Order.*?(\d+)");
                    if (!match.Success) continue;
                    var orderId = match.Groups[1].Value;
                    var orderPrintStatus = printStatuses.FirstOrDefault(x => x.Order.Contains(orderId, StringComparison.InvariantCultureIgnoreCase));
                    if (orderPrintStatus == null)
                    {
                        logger.LogError(new Exception($"Card {card.Name} is in {printedList.Name} but was not found in print queue google sheet"), "Card {CardName} is in {ListName} but was not found in print queue google sheet", card.Name,
                            printedList.Name);
                        continue;
                    }

                    if (orderPrintStatus.IsShipped)
                    {
                        await MoveCardToList(boardId, card, atDans, logger);
                    }
                }
                logger.LogInformation("checked cards in {ListName} to be moved", printedList.Name);
            }
        }

        private static TrelloList GetListByName(IReadOnlyList<TrelloList> lists, string listName, string boardId, ILogger logger)
        {
            var dactylsToPrintList = lists.SingleOrDefault(x => x.Name.Equals(listName, StringComparison.InvariantCultureIgnoreCase));
            if (dactylsToPrintList != null) return dactylsToPrintList;

            var exception = new TrelloBoardListChangedException(boardId, listName);
            logger.LogError(exception, "Trello board {BoardId} changed the Dactyl to Print list name. Current list of names {ListsNames}", boardId, lists.Select(x => x.Name));
            throw exception;
        }

        private async Task MoveCardToList(string boardId, TrelloListCards card, TrelloList list, ILogger logger)
        {
            try
            {
                logger.LogInformation("moving card {CardName} to list {ListName} on board {BoardId}", card.Name, list.Name, boardId);
                await _trelloClient.MoveCardToList(card.Id, list.Id);
            }
            catch (HttpRequestException e)
            {
                logger.LogError(e, "moving card {CardName} to list {ListName} on board {BoardId} was not successful", card.Name, list.Name, boardId);
            }
        }
    }

    public class TrelloBoardListChangedException : Exception
    {
        public TrelloBoardListChangedException(string boardId, string listName)
        :base($"When getting the lists from board {boardId} could not find list with name '{listName}'")
        {
        }
    }
}
