using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PoC.Messaging.Server.Models;
using PoC.Repositories.StorageAccounts;

namespace PoC.Messaging.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly IMapper _mapper;
        private readonly IQueueConnector _queueConnector;

        public MessagesController(
            ILogger<MessagesController> logger,
            IMapper mapper,
            IQueueConnector queueConnector)
        {
            _logger = logger;
            _mapper = mapper;
            _queueConnector = queueConnector;
        }

        [HttpGet("PeekLastMessage")]
        public async Task<QueueMessageDTO> PeekLastMessageFromQueueAsync()
        {
            try
            {

                var message = await _queueConnector.PeekMessageBodyAsync<QueueMessage>();
                return _mapper.Map<QueueMessageDTO>(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpGet("LastMessage")]
        public async Task<QueueMessageDTO> ReceiveLastMessageFromQueueAsync()
        {
            try
            {

                var message = await _queueConnector.ReceiveMessageBodyAsync<QueueMessage>();
                return _mapper.Map<QueueMessageDTO>(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpPost()]
        public async Task<bool> AddMessageToQueueAsync(string message, string name)
        {
            try
            {
                await _queueConnector.AddMessageAsync(new QueueMessage()
                {
                    Text = message,
                    ToUserName = name,
                });

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
