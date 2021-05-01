using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public class ShopifyClientSettings
    {
        public string User { get; set; }
        public string Password { get; set; }
    }
    public static class ShopifyClientSettingsExtensions
    {
        public static IServiceCollection BindShopifyClientSettings(this IServiceCollection services)
        {
            return services.AddOptions<ShopifyClientSettings>()
                .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("Shopify").Bind(settings))
                .Services;
        }
    }
}
