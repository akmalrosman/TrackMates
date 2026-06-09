using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace FriendsMusicTracker.Api.Hubs
{
    public class ChatHub : Hub
    {
        // This receives the message from one user's phone
        public async Task SendMessage(string user, string text)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, text);
        }
    }
}