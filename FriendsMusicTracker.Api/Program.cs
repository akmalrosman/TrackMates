using FriendsMusicTracker.Data;
using Microsoft.EntityFrameworkCore;
using FriendsMusicTracker.Api.Hubs;
// Make sure to add the correct using statement if ChatHub is in a specific folder (like .Hubs)
// using FriendsMusicTracker.Api.Hubs; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔓 Open the gates so your Web App and Mobile App can reach this API safely during development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddControllers();

// 💬 ADD THIS: Register SignalR services for the chat!
builder.Services.AddSignalR();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// 🛑 KEEP THIS COMMENTED OUT for the Android Emulator to work on HTTP!
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// 💬 ADD THIS: Map the SignalR ChatHub to the exact endpoint the mobile app is looking for!
app.MapHub<ChatHub>("/chathub");

app.Urls.Add("http://0.0.0.0:5255");

app.Run();