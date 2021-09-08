using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.ServiceBus;
using NUnit.Framework;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public class FakeMessageBus
    {
        public static Message CreateMessage(string fileRelativePath)
        {
            var file = GetFile(fileRelativePath);
            var message = new Message(File.ReadAllBytes(file.FullName));

            return message;
        }

        public static HttpRequest CreateRequest(string fileRelativePath)
        {
            var file = GetFile(fileRelativePath);
            var defaultHttpRequest = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Body = file.OpenRead()
            };
            return defaultHttpRequest;
        }

        private static FileInfo GetFile(string fileRelativePath)
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, fileRelativePath);
            var file = new FileInfo(path);
            if (!file.Exists) Assert.Inconclusive("The order file could not be found");
            return file;
        }
    }
}
