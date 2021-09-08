using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImmerDiscordBot.TrelloListener.Contracts.Shopify.Models;
using ImmerDiscordBot.TrelloListener.Core.Shopify;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace ImmerDiscordBot.TrelloListener
{
    [TestFixture]
    public class ShopifyOrderCreateTriggerTests
    {
        [TestCase("data/order-3995.json")]
        [TestCase("data/order-3996.json")]
        [TestCase("data/order-3997.json")]
        [TestCase("data/order-3998.json")]
        public async Task CanReadAndSendOrder(string path)
        {
            var context = await RunCode(path);

            AssertNoErrorCollected(context);
            Assert.That(context.CollectedOrders, Is.Not.Empty & Has.Count.EqualTo(1));
            var order = context.CollectedOrders[0];
            Assert.That(order, Is.Not.Null);
        }

        [TestCase("data/order-3994.json")]
        public async Task OrderIsCancelled(string path)
        {
            var context = await RunCode(path);

            AssertNoErrorCollected(context);
            Assert.That(context.CollectedOrders, Is.Empty, "Order 3994 is cancelled and should not be collected");
        }

        private static async Task<Context> RunCode(string path)
        {
            var request = FakeMessageBus.CreateRequest(path);
            var service = new ShopifyOrderCreateTrigger(new OrderJsonReader(), null);

            using var source = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var messageCollector = new TestMessageCollector<Order>();
            var errorMessageCollector = new TestMessageCollector<ShopifyOrderCreateTrigger.ErrorContext>();
            var result = await service.RunAsync(request, messageCollector, errorMessageCollector, NullLogger.Instance, source.Token);
            Assert.That(result, Is.TypeOf<OkResult>());
            await messageCollector.FlushAsync(source.Token);
            return new Context
            {
                CollectedOrders = messageCollector.Items.AsReadOnly(),
                CollectedErrorContexts = errorMessageCollector.Items.AsReadOnly()
            };
        }

        private static void AssertNoErrorCollected(Context context)
        {
            if (context.CollectedErrorContexts.Any())
            {
                throw context.CollectedErrorContexts[0].Exception;
            }
        }

        private class Context
        {
            public ReadOnlyCollection<Order> CollectedOrders { get; set; }
            public ReadOnlyCollection<ShopifyOrderCreateTrigger.ErrorContext> CollectedErrorContexts { get; set; }
        }

        private class TestMessageCollector<T> : IAsyncCollector<T>
        {
            public List<T> Items { get; } = new List<T>();
            private readonly ConcurrentQueue<T> _ordersCache = new ConcurrentQueue<T>();

            public async Task AddAsync(T item, CancellationToken cancellationToken = new CancellationToken())
            {
                _ordersCache.Enqueue(item);
            }

            public async Task FlushAsync(CancellationToken cancellationToken = new CancellationToken())
            {
                lock (Items)
                {
                    while (_ordersCache.Count > 0)
                    {
                        var tryDequeue = _ordersCache.TryDequeue(out var order);
                        if (tryDequeue)
                        {
                            Items.Add(order);
                        }
                    }
                }
            }
        }
    }
}
