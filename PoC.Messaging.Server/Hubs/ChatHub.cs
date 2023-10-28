using Microsoft.AspNetCore.SignalR;

namespace PoC.Messaging.Server.Hubs
{
    public class ChatHub : Hub
    {
        const string MESSAGE_RECEIVED = "MESSAGE_RECEIVED";

        [HubMethodName("SEND_MESSAGE_TO_ALL")]
        public async Task SendMessageToAll(string user, string message)
        {
            await Clients.All.SendAsync(MESSAGE_RECEIVED, user, message);
        }

        [HubMethodName("SEND_MESSAGE_TO_ME")]
        public async Task SendMeMessage(string user, string message)
        {
            await Clients.Caller.SendAsync(MESSAGE_RECEIVED, user, message);
        }

        [HubMethodName("SEND_MESSAGE_TO_OTHERS")]
        public async Task SendOthersMessage(string user, string message)
        {
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
