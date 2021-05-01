using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImmerDiscordBot.TrelloListener.Core.Trello
{
    public class TrelloClientSettings
    {
        public string Key { get; set; }
        public string Token { get; set; }
    }
    public static class TrelloClientSettingsExtensions
    {
        public static IServiceCollection BindTrelloClientSettings(this IServiceCollection services)
        {
            return services.AddOptions<TrelloClientSettings>()
                .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("Trello").Bind(settings))
                .Services;
        }
    }
}
