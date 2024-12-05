using System.Text.Json.Serialization;

namespace Prova_WebHook.DTO
{

    public class DataJson
    {
        [JsonPropertyName("version")]
        public string version { get; set; }

        [JsonPropertyName("packageName")]
        public string packageName { get; set; }

        [JsonPropertyName("eventTimeMillis")]
        public string eventTimeMillis { get; set; }

        [JsonPropertyName("subscriptionNotification")]
        public SubscriptionNotification subscriptionNotification { get; set; }
    }
    public class SubscriptionNotification
    {
        [JsonPropertyName("version")]
        public string version { get; set; }
        [JsonPropertyName("notificationType")]
        public int notificationType { get; set; }
        [JsonPropertyName("purchaseToken")]
        public string purchaseToken { get; set; }
        [JsonPropertyName("subscriptionId")]
        public string subscriptionId { get; set; }
    }

}
