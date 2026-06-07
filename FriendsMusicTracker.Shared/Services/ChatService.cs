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
            // Both platforms now point to the correct HTTP port 5255!
            string serverUrl = OperatingSystem.IsAndroid()
                ? "http://192.168.1.40:5255/chathub"
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

                // This triggers the UI component to redraw itself
                OnMessageAdded?.Invoke();
            });

            // Start the connection in the background cleanly
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

        // Turned into an async Task so it waits for the transmission to finish
        public async Task SendMessage(string user, string text)
        {
            if (string.IsNullOrWhiteSpace(text) || _hubConnection is null) return;

            try
            {
                // Make sure we are connected before sending
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