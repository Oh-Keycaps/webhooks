using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImmerDiscordBot.TrelloListener.Core.GoogleSheets
{
    public class GoogleSheetsSettings
    {
        public string DocumentId { get; set; }
        public string SheetId { get; set; }
        public string ServiceAccountCredentialFile { get; set; }
    }

    public static class GoogleSheetsSettingsExtensions
    {
        public static IServiceCollection AddGoogleSheetsServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<SheetsServiceProvider>()
                .AddTransient<SheetsClient>()
                .AddOptions<GoogleSheetsSettings>()
                .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("GoogleSheets").Bind(settings))
                .Services;
        }
    }
}
