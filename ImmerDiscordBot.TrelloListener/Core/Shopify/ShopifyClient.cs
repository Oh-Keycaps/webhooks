using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;

namespace ImmerDiscordBot.TrelloListener.Core.Shopify
{
    public class ShopifyClient : IShopifyClient
    {
        private readonly HttpClient _client;

        public ShopifyClient(IOptions<ShopifyClientSettings> options)
        {
            var settings = options.Value;
            var b = Encoding.UTF8.GetBytes($"{settings.User}:{settings.Password}");
            _client = new HttpClient {BaseAddress = new Uri("https://mechcaps.myshopify.com")};
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(b));
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }

    public interface IShopifyClient : IDisposable
    {
    }
}
