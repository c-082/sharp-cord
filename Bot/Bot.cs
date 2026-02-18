using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;

using SharpCord.Bot.Handlers;
using SharpCord.Bot.Services;

namespace SharpCord.Bot;

internal class Bot
{
    private readonly DiscordSocketClient _client;
    private readonly IServiceProvider _services;
    private readonly UtilityService _utility;

    public Bot()
    {
        _client = new(new()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        });
        _utility = new();
        _services = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_utility)
            .BuildServiceProvider();
    }
    public async Task RunAsync()
    {
        var handler = new CommandHandler(_client, _services);
        await handler.InitializeAsync();

        var token = Environment.GetEnvironmentVariable("BOT_TOKEN");

        if (string.IsNullOrWhiteSpace(token))
        {
            await Console.Error.WriteLineAsync("CRITICAL: Bot token not found!");
            return;
        }

        _client.Log += LogAsync;

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        await Task.Delay(-1);
    }

    private async Task LogAsync(LogMessage message) =>
        await Console.Out.WriteLineAsync(message.ToString());

}