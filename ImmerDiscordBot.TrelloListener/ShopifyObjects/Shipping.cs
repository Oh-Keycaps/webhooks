using Newtonsoft.Json;

namespace ImmerDiscordBot.TrelloListener.ShopifyObjects
{
    public class Shipping
    {
        /// <summary>
        /// Whether to refund all remaining shipping.
        /// </summary>
        [JsonProperty("full_refund")]
        public bool? FullRefund { get; set; }

        /// <summary>
        /// Set a specific amount to refund for shipping. Takes precedence over full_refund.
        /// </summary>
        [JsonProperty("amount")]
        public decimal? Amount { get; set; }

        /// <summary>
        /// The maximum amount that can be refunded
        /// </summary>
        [JsonProperty("maximum_refundable")]
        public decimal? MaximumRefundable { get; set; }
    }
}
