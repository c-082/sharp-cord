using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace SharpCord.Bot.Services;

internal static class KeepAlive
{
    public static void Start()
    {
        var builder = WebApplication.CreateBuilder();

        var app = builder.Build();
        app.MapGet("/", () => Results.Ok("The bot is running!"));

        var port = Environment.GetEnvironmentVariable("PORT");
        app.Urls.Add($"http://0.0.0.0:{port}");
        _ = app.RunAsync();
    }
}