using Azure.Storage.Queues;
using System.Text.Json;

namespace PoC.Repositories.StorageAccounts
{
    public interface IQueueConnector
    {
        static TimeSpan DefaultVisibilityTimeout { get; }
        static TimeSpan DefaultTimeToLive { get; }

        Task<T> PeekMessageBodyAsync<T>();
        Task<T> ReceiveMessageBodyAsync<T>();
        Task AddMessageAsync<T>(T body);
    }

    public class QueueConnector(string connectionString, string queueName) : IQueueConnector
    {
        private const string CONNECTION_STRING = "BlobEndpoint=https://pocm.blob.core.windows.net/;QueueEndpoint=https://pocm.queue.core.windows.net/;FileEndpoint=https://pocm.file.core.windows.net/;TableEndpoint=https://pocm.table.core.windows.net/;SharedAccessSignature=sv=2022-11-02&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2024-11-01T22:52:55Z&st=2023-11-01T14:52:55Z&sip=1.1.1.1-255.255.255.255&spr=https&sig=rWA9NBTG7QaFk5oWraYHLmFtadCli%2F982A4j4O5w8Qc%3D";
        private const string QUEUE_NAME = "messages";

        private readonly QueueClient _queueClient = new QueueClient(connectionString, queueName);

        public QueueConnector() : this(CONNECTION_STRING, QUEUE_NAME) { }

        public static TimeSpan DefaultVisibilityTimeout => TimeSpan.FromTicks(0);
        public static TimeSpan DefaultTimeToLive => TimeSpan.FromDays(7);

        public async Task<T> PeekMessageBodyAsync<T>()
        {
            var message = await _queueClient.PeekMessageAsync();
            var value = message.Value;
            var body = value.Body;
            body.ToObjectFromJson<T>();

            return body.ToObjectFromJson<T>();
        }

        public async Task<T> ReceiveMessageBodyAsync<T>()
        {
            var message = await _queueClient.ReceiveMessageAsync();
            //TODO: handle no message available
            var value = message.Value;
            var body = value.Body;
            body.ToObjectFromJson<T>();

            return body.ToObjectFromJson<T>();
        }

        public async Task AddMessageAsync<T>(T body)
        {
            string serializedBody = JsonSerializer.Serialize(body);
            var response = await _queueClient.SendMessageAsync(
                messageText: serializedBody,
                visibilityTimeout: DefaultVisibilityTimeout,
                timeToLive: DefaultTimeToLive,
                cancellationToken: new CancellationToken()
            );
        }
    }
}
