using System.IO;
using Microsoft.Azure.ServiceBus;
using NUnit.Framework;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public class FakeMessageBus
    {
        public static Message CreateMessage(string fileRelativePath)
        {
            var file = Path.Combine(TestContext.CurrentContext.TestDirectory, fileRelativePath);
            if (!File.Exists(file)) Assert.Inconclusive("The order file could not be found");
            var message = new Message(File.ReadAllBytes(file));

            return message;
        }
    }
}
