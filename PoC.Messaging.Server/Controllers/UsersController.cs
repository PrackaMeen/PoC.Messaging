using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using PoC.Repositories.StorageAccounts;

namespace PoC.Messaging.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;
        private readonly ITableConnector<User> _tableConnector;

        public UsersController(
            ILogger<UsersController> logger,
            IMapper mapper,
            ITableConnector<User> tableConnector)
        {
            _logger = logger;
            _mapper = mapper;
            _tableConnector = tableConnector;
        }


        [HttpGet("")]
        public async Task<User> GetUserBy(string partitionId, string rowId)
        {
            try
            {
                var user = await _tableConnector.GetItemBy(partitionId, rowId);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpGet("a")]
        public async Task<Page<User>> GetAllUsers(int pageSize = 50)
        {
            try
            {
                var users = _tableConnector.GetItems(pageSize);
                await foreach (var user in users)
                {
                    return user;
                }

                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpPost()]
        public async Task<User> CreateUser(string user)
        {
            try
            {
                var newUser = new User()
                {
                    UserId = user
                };
                await _tableConnector.CreateItem(newUser);
                var createdUser = await _tableConnector.GetItemBy(newUser.PartitionKey, newUser.RowKey);

                return createdUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }

    public class User : ITableItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string PartitionKey { get; set; } = Guid.NewGuid().ToString();
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.UtcNow;
        public ETag ETag { get; set; } = ETag.All;
        public string UserId { get; set; } = string.Empty;
    }
}
