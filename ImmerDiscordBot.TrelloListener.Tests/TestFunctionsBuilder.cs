using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ImmerDiscordBot.TrelloListener
{
    internal class TestFunctionsBuilder : IFunctionsHostBuilder
    {
        class LocalSettings
        {
            public Dictionary<string, string> Values { get; set; }
        }

        public IServiceCollection Services { get; private set; }

        public IServiceProvider Build()
        {
            var localSettingsPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "local.settings.json");
            if (!File.Exists(localSettingsPath))
            {
                throw new FileNotFoundException("The configuration file was not found and is not optional", localSettingsPath);
            }
            var readAllText = File.ReadAllText(localSettingsPath);
            var settings = JsonConvert.DeserializeObject<LocalSettings>(readAllText);
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(settings.Values)
                .Build();

            Services = new ServiceCollection()
                .AddSingleton<IConfiguration>(config);
            new Startup().Configure(this);

            return Services.BuildServiceProvider();
        }
    }
}
