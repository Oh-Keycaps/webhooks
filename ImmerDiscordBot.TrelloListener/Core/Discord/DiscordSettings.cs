using ImmerDiscordBot.TrelloListener.Core.Shopify;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImmerDiscordBot.TrelloListener.Core.Discord
{
    public class DiscordSettings
    {
        public string WebhookId { get; set; }
        public string WebhookToken { get; set; }
    }
    public static class DiscordSettingsExtensions
    {
        public static IServiceCollection BindDiscordSettings(this IServiceCollection services)
        {
            return services.AddOptions<DiscordSettings>()
                .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("Discord").Bind(settings))
                .Services;
        }
    }

}
