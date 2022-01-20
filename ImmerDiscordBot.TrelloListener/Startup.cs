using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using ImmerDiscordBot.TrelloListener.Core;
using ImmerDiscordBot.TrelloListener.Core.Discord;
using ImmerDiscordBot.TrelloListener.Core.GoogleSheets;
using ImmerDiscordBot.TrelloListener.Core.Shopify;
using ImmerDiscordBot.TrelloListener.Core.Trello;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(ImmerDiscordBot.TrelloListener.Startup))]

namespace ImmerDiscordBot.TrelloListener
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .BindTrelloClientSettings()
                .BindDiscordSettings()
                .BindShopifyClientSettings()
                .AddGoogleSheetsServices()
                .AddTransient<DiscordWebHook>()
                .AddTransient<DiscordMessageBuilder>()
                .AddSingleton<TrelloClient>()
                .AddSingleton<TrelloUserService>()
                .AddTransient<IOrderReader, OrderJsonReader>()
                .AddTransient<IOrderConverter, OrderConverter>()
                .AddTransient<IOrderFilter, OrderCreatedFilter>()
                .AddTransient<IOrderToTrelloCardMapper, OrderToTrelloCardMapper>()
                .AddSingleton<IShopifyClient, ShopifyClient>()
                .AddTransient<ShopifyServiceBusTriggerManager>()
                .AddTransient<OrderToSheetRowMapper>()
                .AddTransient<OrderPrintStatusProvider>()
                ;
        }
    }
}
