using Microsoft.AspNetCore.SignalR;
using PoC.Messaging.Server.Models;
using PoC.Repositories.StorageAccounts;

namespace PoC.Messaging.Server.Hubs
{
    public class ChatHub : Hub
    {
        const string MESSAGE_RECEIVED = "MESSAGE_RECEIVED";
        const string SEND_MESSAGE_TO_ALL = "SEND_MESSAGE_TO_ALL";
        const string SEND_MESSAGE_TO_ME = "SEND_MESSAGE_TO_ME";
        const string SEND_MESSAGE_TO_OTHERS = "SEND_MESSAGE_TO_OTHERS";
        const string GET_ALL_CONNECTED_USERS = "GET_ALL_CONNECTED_USERS";

        private readonly ILogger _logger;
        private readonly IQueueConnector _queueConnector;
        public ChatHub(ILogger logger, IQueueConnector queueConnector)
        {
            _logger = logger;
            _queueConnector = queueConnector;
        }

        [HubMethodName(SEND_MESSAGE_TO_ALL)]
        public async Task SendMessageToAll(string user, string message)
        {
            var queueMessage = new QueueMessage
            {
                Text = message,
                ToUserName = user,
                FromUserName = this.Context.UserIdentifier ?? string.Empty
            };

            await _queueConnector.AddMessageAsync(queueMessage);
            await Clients.All.SendAsync(MESSAGE_RECEIVED, user, message);
        }

        [HubMethodName(SEND_MESSAGE_TO_ME)]
        public async Task SendMeMessage(string user, string message)
        {
            var queueMessage = new QueueMessage
            {
                Text = message,
                ToUserName = user,
                FromUserName = this.Context.UserIdentifier ?? string.Empty
            };

            await _queueConnector.AddMessageAsync(queueMessage);
            await Clients.Caller.SendAsync(MESSAGE_RECEIVED, user, message);
        }

        [HubMethodName(SEND_MESSAGE_TO_OTHERS)]
        public async Task SendOthersMessage(string user, string message)
        {
            var queueMessage = new QueueMessage
            {
                Text = message,
                ToUserName = user,
                FromUserName = this.Context.UserIdentifier ?? string.Empty
            };

            await _queueConnector.AddMessageAsync(queueMessage);
            await Clients.Others.SendAsync(MESSAGE_RECEIVED, user, message);
            //await Clients.Groups(user).SendAsync(MESSAGE_RECEIVED, user, message);
        }

        [HubMethodName(GET_ALL_CONNECTED_USERS)]
        public async Task GetAllConnectedUsers(string user, string message)
        {
            var queueMessage = new QueueMessage
            {
                Text = message,
                ToUserName = user,
                FromUserName = this.Context.UserIdentifier ?? string.Empty
            };

            await _queueConnector.AddMessageAsync(queueMessage);
            await Clients.Others.SendAsync(MESSAGE_RECEIVED, user, message);
        }

        public async Task AddToGroup(string groupName, string connectionId)
        {
            await Groups.AddToGroupAsync(connectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName, string connectionId)
        {
            await Groups.RemoveFromGroupAsync(connectionId, groupName);
        }
    }
}
