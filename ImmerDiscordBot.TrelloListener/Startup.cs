using ImmerDiscordBot.TrelloListener.Core;
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

            builder.Services
                .AddTransient<DiscordWebHook>()
                .AddTransient<DiscordMessageBuilder>();
        }
    }
}
