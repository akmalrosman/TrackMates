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

            // =====================================================================
            // 🚨 TEAM MEMBER INSTRUCTIONS:
            // If you are testing on an Android emulator or physical device, 
            // change the IP address below to YOUR computer's local IPv4 address!
            // (Windows: Open CMD -> type 'ipconfig' -> look for IPv4 Address)
            // (Mac: Open Terminal -> type 'ifconfig' -> look for inet under en0)
            // =====================================================================

            string myIpAddress = "YOUR_IPV4_ADDRESS_HERE";

            string apiUrl = OperatingSystem.IsAndroid()
                ? $"http://{myIpAddress}:5255/"
                : "https://localhost:7164/";

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });

            // Register the UserState so the mobile app knows who is logged in
            builder.Services.AddScoped<FriendsMusicTracker.Data.UserState>();

            // Register the Chat Service so the global layout chat functions properly
            builder.Services.AddSingleton<FriendsMusicTracker.Services.ChatService>();

#if ANDROID
            // ALLOW YOUTUBE AUTOPLAY ON ANDROID:
            // Overriding the default native Android web view restriction to allow autoplay hooks
            Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebViewHandler.BlazorWebViewMapper.AppendToMapping("EnableAutoplayFix", (handler, view) =>
            {
                handler.PlatformView.Settings.MediaPlaybackRequiresUserGesture = false;
            });
#endif

            return builder.Build();
        }
    }
}