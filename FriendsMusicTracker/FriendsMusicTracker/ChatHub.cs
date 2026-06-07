using Microsoft.AspNetCore.SignalR;

namespace FriendsMusicTracker
{
    public class ChatHub : Hub
    {
        // The mobile app calls this, and the Hub shouts it out to EVERYONE connected!
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}