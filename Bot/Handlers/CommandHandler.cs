using Discord.WebSocket;
using Discord.Commands;
using Discord.Interactions;

namespace SharpCord.Bot.Handlers;

internal class CommandHandler(
    DiscordSocketClient client,
    CommandService commands,
    InteractionService interactions,
    IServiceProvider? services)
{
    private readonly DiscordSocketClient _client = client;
    private readonly CommandService _commands = commands;
    private readonly IServiceProvider? _services = services;
    private readonly InteractionService _interactions = interactions;

    public async Task InitializeAsync()
    {
        _client.MessageReceived += HandleMessageAsync;

        _client.InteractionCreated += HandleInteractionAsync;

        await _commands.AddModulesAsync(
            typeof(CommandHandler).Assembly,
            _services
        );

        await _interactions.AddModulesAsync(
            typeof(CommandHandler).Assembly,
            _services
        );

        _client.Ready += async () =>
        {
            await _interactions.AddCommandsGloballyAsync();
        };
    }
    private async Task HandleMessageAsync(SocketMessage rawMessage)
    {
        if (rawMessage is not SocketUserMessage message)
        {
            return;
        }

        if (message.Author.IsBot)
        {
            return;
        }

        var argPos = 0;

        if (!message.HasCharPrefix('$', ref argPos))
        {
            return;
        }

        while (argPos < message.Content.Length && message.Content[argPos] == ' ')
        {
            argPos++;
        }

        var context = new SocketCommandContext(_client, message);
        await _commands.ExecuteAsync(context, argPos, _services);
    }

    private async Task HandleInteractionAsync(SocketInteraction interaction)
    {
        var context = new SocketInteractionContext(_client, interaction);
        await _interactions.ExecuteCommandAsync(context, _services);
    }
}