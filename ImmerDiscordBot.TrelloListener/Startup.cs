using ImmerDiscordBot.TrelloListener.Contracts.Shopify;
using ImmerDiscordBot.TrelloListener.Core;
using ImmerDiscordBot.TrelloListener.Core.Discord;
using ImmerDiscordBot.TrelloListener.Core.Shopify;
using ImmerDiscordBot.TrelloListener.Core.Trello;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(ImmerDiscordBot.TrelloListener.Startup))]

namespace ImmerDiscordBot.TrelloListener
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<DiscordSettings>()
                .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("Discord").Bind(settings));
            builder.Services.AddOptions<TrelloClientSettings>()
                .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("Trello").Bind(settings));
            builder.Services.AddOptions<ShopifyClientSettings>()
                .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("Shopify").Bind(settings));

            builder.Services
                .AddTransient<DiscordWebHook>()
                .AddTransient<DiscordMessageBuilder>()
                .AddSingleton<TrelloClient>()
                .AddSingleton<TrelloUserService>()
                .AddTransient<IOrderReader, OrderJsonReader>()
                .AddTransient<IOrderFilter, OrderCreatedFilter>()
                .AddTransient<IOrderToTrelloCardMapper, OrderToTrelloCardMapper>()
                .AddSingleton<IShopifyClient, ShopifyClient>()
                ;
        }
    }
}
