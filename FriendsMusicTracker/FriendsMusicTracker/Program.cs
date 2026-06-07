using FriendsMusicTracker;
using FriendsMusicTracker.Components;
using FriendsMusicTracker.Data;
using FriendsMusicTracker.Services;

var builder = WebApplication.CreateBuilder(args);

// POINT WEBSITE TO THE BACKEND API:
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5255/") });

// Register the User State service so the app knows who is logged in
builder.Services.AddScoped<FriendsMusicTracker.Data.UserState>();

// Register the Chat Service so the global layout chat functions properly
builder.Services.AddSingleton<FriendsMusicTracker.Services.ChatService>();
builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(FriendsMusicTracker.Shared.Pages.Songs).Assembly);

app.MapHub<ChatHub>("/chathub");

app.Run();