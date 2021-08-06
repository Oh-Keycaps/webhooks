using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.ShopifyObjects;
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

        public async Task<Order> GetOrder(long orderId)
        {
            var url = $"/admin/api/2021-01/orders/{orderId}.json";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var order = Newtonsoft.Json.JsonConvert.DeserializeObject<ShopifyOrderResponse>(content);
            return order.Order;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }

    public class ShopifyOrderResponse
    {
        public Order Order {get;set;}
    }

    public interface IShopifyClient : IDisposable
    {
        Task<Order> GetOrder(long orderId);
    }
}
