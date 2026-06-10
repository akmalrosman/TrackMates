using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace FriendsMusicTracker.Services
{
    public class ChatMessage
    {
        public string User { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }

    public class ChatService
    {
        private readonly List<ChatMessage> _messages = new();
        public IReadOnlyList<ChatMessage> Messages => _messages;

        public event Action? OnMessageAdded;

        private HubConnection? _hubConnection;

        public ChatService()
        {
            // TYPE IPV4_ADDRESS HERE
            string myIpAddress = "HERE_IPV4_ADDRESS";

            string serverUrl = OperatingSystem.IsAndroid()
                ? $"http://{myIpAddress}:5255/chathub"
                : "http://localhost:5255/chathub";

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(serverUrl)
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<string, string>("ReceiveMessage", (user, text) =>
            {
                _messages.Add(new ChatMessage
                {
                    User = user,
                    Text = text,
                    Timestamp = DateTime.Now
                });

                if (_messages.Count > 50) _messages.RemoveAt(0);

                OnMessageAdded?.Invoke();
            });

            Task.Run(async () =>
            {
                try
                {
                    await _hubConnection.StartAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SignalR Connection Error: {ex.Message}");
                }
            });
        }

        public async Task SendMessage(string user, string text)
        {
            if (string.IsNullOrWhiteSpace(text) || _hubConnection is null) return;

            try
            {
                if (_hubConnection.State == HubConnectionState.Disconnected)
                {
                    await _hubConnection.StartAsync();
                }

                await _hubConnection.InvokeAsync("SendMessage", user, text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }
    }
}