using Chat.Api.Models;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Api.Hubs;

public class ChatHub: Hub
{
    public async Task SendMessage(Message message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}
