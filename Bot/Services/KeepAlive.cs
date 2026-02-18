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

        _ = app.RunAsync();
    }
}