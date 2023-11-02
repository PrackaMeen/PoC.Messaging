using System.Text.Json.Serialization;

namespace PoC.Messaging.Server.Models
{
    internal class QueueMessage
    {
        [JsonPropertyName("t")]
        public string Text { get; set; } = "";

        [JsonPropertyName("fu")]
        public string FromUserName { get; set; } = "";

        [JsonPropertyName("tu")]
        public string ToUserName { get; set; } = "";
    }

    public class QueueMessageDTO
    {
        public string Text { get; set; } = "";
        public string FromUserName { get; set; } = "";
        public string ToUserName { get; set; } = "";

        public static QueueMessageDTO Empty => new();
    }
}
