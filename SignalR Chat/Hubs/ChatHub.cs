using Microsoft.AspNetCore.SignalR;
using SignalR_Chat.Dtos;
using SignalR_Chat.Services;

namespace SignalR_Chat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;
        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "ChatUsers");
            await Clients.Caller.SendAsync("UserConnected", Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "ChatUsers");
            var user = _chatService.GetUserByConnectionId(Context.ConnectionId);
            _chatService.RemoveUser(user);
            await DisplayOnlineUsers();
            await base.OnDisconnectedAsync(exception);
        }

        public async Task AddUserConnectionId(string name)
        {
            _chatService.AddUserConnectionId(name, Context.ConnectionId);
            await DisplayOnlineUsers();
        }

        private async Task DisplayOnlineUsers()
        {
            var onlineUsers = _chatService.GetOnlineUsers();
            await Clients.Group("ChatUsers").SendAsync("OnlineUsers", onlineUsers);

        }

        public async Task SendMessage(string message)
        {
            var user = _chatService.GetUserByConnectionId(Context.ConnectionId);
            await Clients.Group("ChatUsers").SendAsync("ChatMessages", new MessageDto(message, user));
        }

        public async Task SendPrivateMessage(string message, string user)
        {
            var from = _chatService.GetUserByConnectionId(Context.ConnectionId);
            var group = GetPrivateGroupName(from, user);
            await Clients.Group(group).SendAsync("NewPrivateMessage", new MessageDto(message, from, user));
        }

        public async Task CreatePrivateChat(string user)
        {
            var from = _chatService.GetUserByConnectionId(Context.ConnectionId);
            string privateGroupName = GetPrivateGroupName(from, user);
            await Groups.AddToGroupAsync(Context.ConnectionId, privateGroupName);
            var toConnectionId = _chatService.GetConnectionIdByUser(user);
            await Groups.AddToGroupAsync(toConnectionId, privateGroupName);
        }

        private string GetPrivateGroupName(string from, string to)
        {
            var stringCompare = string.CompareOrdinal(from, to) < 0;
            return stringCompare ? $"{from}-{to}" : $"{to}-{from}";
        }

        public async Task LeaveChat(string from, string to)
        {
            string privateGroupName = GetPrivateGroupName(from, to);
            await Clients.Group(privateGroupName).SendAsync("LeaveChat", from);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, privateGroupName);
        }
    }
}
