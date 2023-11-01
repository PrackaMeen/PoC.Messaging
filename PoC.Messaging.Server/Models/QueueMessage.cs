using System.Text.Json.Serialization;

namespace PoC.Messaging.Server.Models
{
    internal class QueueMessage
    {
        [JsonPropertyName("t")]
        public string Text { get; set; } = "";

        [JsonPropertyName("u")]
        public string UserName { get; set; } = "";
    }

    public class QueueMessageDTO
    {
        public string Text { get; set; } = "";
        public string UserName { get; set; } = "";

        public static QueueMessageDTO Empty => new();
    }
}
