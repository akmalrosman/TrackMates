using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace FriendsMusicTracker.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            // TYPE IPV4_ADDRESS HERE
            string myIpAddress = "YOUR_IPV4_HERE";

            string apiUrl = OperatingSystem.IsAndroid()
                ? $"http://{myIpAddress}:5255/"
                : "https://localhost:7164/";

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });

            builder.Services.AddScoped<FriendsMusicTracker.Data.UserState>();

            builder.Services.AddSingleton<FriendsMusicTracker.Services.ChatService>();

#if ANDROID
            Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebViewHandler.BlazorWebViewMapper.AppendToMapping("EnableAutoplayFix", (handler, view) =>
            {
                handler.PlatformView.Settings.MediaPlaybackRequiresUserGesture = false;
            });
#endif

            return builder.Build();
        }
    }
}