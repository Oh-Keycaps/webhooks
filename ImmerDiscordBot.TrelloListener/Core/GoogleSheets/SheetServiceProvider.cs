using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using Microsoft.Extensions.Options;

namespace ImmerDiscordBot.TrelloListener.Core.GoogleSheets
{
    public class SheetsServiceProvider
    {
        private const string ApplicationName = "oh-keycaps shopify webhook";
        private readonly string[] _scopes = {SheetsService.Scope.Spreadsheets};
        private readonly Lazy<SheetsService> _serviceCache;
        private readonly GoogleSheetsSettings _settings;

        public SheetsServiceProvider(IOptions<GoogleSheetsSettings> options)
        {
            _settings = options.Value;
            _serviceCache = new Lazy<SheetsService>(Create);
        }

        public SheetsService Get()
        {
            return _serviceCache.Value;
        }

        private SheetsService Create()
        {
            return new SheetsService(new BaseClientService.Initializer()
            {
                ApplicationName = ApplicationName,
                HttpClientInitializer = GoogleCredential.FromJson(_settings.ServiceAccountCredentialFile).CreateScoped(_scopes)
            });
        }
    }
}
