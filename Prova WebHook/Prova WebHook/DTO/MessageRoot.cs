namespace Prova_WebHook.DTO
{
    using System.Text.Json.Serialization;

    public class Root
    {
        [JsonPropertyName("message")]
        public Message message { get; set; }
        [JsonPropertyName("subscription")]
        public string subscription { get; set; }
    }
    public class Attributes
    {
        [JsonPropertyName("key")]
        public string key { get; set; }
    }
    public class Message
    {
        [JsonPropertyName("attributes")] public Attributes attributes { get; set; }
        [JsonPropertyName("data")] public string data { get; set; }
        [JsonPropertyName("messageId")] public string messageId { get; set; }
    }

}
