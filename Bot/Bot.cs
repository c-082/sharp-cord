using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;

using SharpCord.Bot.Handlers;
using SharpCord.Bot.Services;

namespace SharpCord.Bot;

internal class Bot(DiscordSocketClient client, CommandHandler handler)
{
    private readonly DiscordSocketClient _client = client;
    private readonly CommandHandler _handler = handler;

    public async Task RunAsync()
    {
        await _handler.InitializeAsync();

        var token = Environment.GetEnvironmentVariable("BOT_TOKEN");

        if (string.IsNullOrWhiteSpace(token))
        {
            await Console.Error.WriteLineAsync("CRITICAL: Bot token not found!");
            return;
        }

        _client.Log += LogAsync;

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        _client.Ready += async () =>
            await Console.Out.WriteLineAsync($"Logged in as {_client.CurrentUser}");

        await Task.Delay(-1);
    }

    private async Task LogAsync(LogMessage message) =>
        await Console.Out.WriteLineAsync(message.ToString());

}