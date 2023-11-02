using Azure;
using Azure.Data.Tables;
using System.Text.Json;

namespace PoC.Repositories.StorageAccounts
{
    public interface ITableItem : ITableEntity
    {
        Guid Id { get; }
    }

    public interface ITableConnector<T> where T : class, ITableItem
    {
        static TimeSpan DefaultVisibilityTimeout { get; }
        static TimeSpan DefaultTimeToLive { get; }

        Task<bool> CreateItem(T item);
        Task<T> GetItemBy(string partitionKey, string rowId);
        IAsyncEnumerable<Page<T>> GetItems(int pageSize);
    }

    public class TableConnector<T>(string connectionString, string queueName) : ITableConnector<T> where T : class, ITableItem
    {
        private const string CONNECTION_STRING = "BlobEndpoint=https://pocm.blob.core.windows.net/;QueueEndpoint=https://pocm.queue.core.windows.net/;FileEndpoint=https://pocm.file.core.windows.net/;TableEndpoint=https://pocm.table.core.windows.net/;SharedAccessSignature=sv=2022-11-02&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2024-11-01T22:52:55Z&st=2023-11-01T14:52:55Z&sip=1.1.1.1-255.255.255.255&spr=https&sig=rWA9NBTG7QaFk5oWraYHLmFtadCli%2F982A4j4O5w8Qc%3D";
        private const string TABLE_NAME = "users";

        private readonly TableClient _tableClient = new(connectionString, queueName);

        public TableConnector() : this(CONNECTION_STRING, TABLE_NAME) { }

        public static TimeSpan DefaultVisibilityTimeout => TimeSpan.FromTicks(0);
        public static TimeSpan DefaultTimeToLive => TimeSpan.FromDays(7);

        public async Task<string> CreateTableIfNotExists()
        {
            var response = await _tableClient.CreateIfNotExistsAsync();
            return response.Value.Name;
        }

        public async Task<bool> CreateItem(T item)
        {
            var message = await _tableClient.AddEntityAsync(item);
            //TODO: handle no message available
            return message.IsError;
        }

        public async Task<T> GetItemBy(string partitionKey, string rowId)
        {
            var item = await _tableClient.GetEntityAsync<T>(partitionKey, rowId);
            return item.Value;
        }

        public IAsyncEnumerable<Page<T>> GetItems(int pageSize)
        {
            var items = _tableClient
                .QueryAsync<T>((_) => true, maxPerPage: pageSize)
                .AsPages();

            return items;
        }
    }
}
