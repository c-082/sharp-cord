using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace SharpCord.Bot.Services;

internal static class KeepAlive
{
    public static void Start()
    {
        var builder = WebApplication.CreateBuilder();

        var app = builder.Build();
        app.MapMethods("/", ["HEAD"], () => Results.Ok("The bot is running!"));

        var portEnv = Environment.GetEnvironmentVariable("PORT");

        if (!ushort.TryParse(portEnv, out var port))
        {
            port = 8080;
        }

        app.Urls.Add($"http://0.0.0.0:{port}");

        _ = app.RunAsync();
    }
}