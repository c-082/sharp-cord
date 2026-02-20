using Discord.WebSocket;
using Discord.Commands;
using Discord.Interactions;
using Discord;

using System.Windows.Input;

namespace SharpCord.Bot.Handlers;

internal class CommandHandler(
    DiscordSocketClient client,
    CommandService commands,
    InteractionService interactions,
    IServiceProvider? services)
{
    private readonly DiscordSocketClient _client = client;
    private readonly CommandService _commands = commands;
    private readonly InteractionService _interactions = interactions;
    private readonly IServiceProvider? _services = services;

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
            await _interactions.RegisterCommandsGloballyAsync();
        };

        _commands.CommandExecuted += CommandExceutedAsync;
        _interactions.SlashCommandExecuted += SlashCommandExecutedAsync;
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

    private async Task CommandExceutedAsync(
        Optional<CommandInfo> command,
        ICommandContext context,
        Discord.Commands.IResult result
    )
    {
        if (!command.IsSpecified)
        {
            return;
        }

        if (result.IsSuccess)
        {
            return;
        }

        switch (result.Error)
        {
            case CommandError.UnmetPrecondition:
                await context.Channel.SendMessageAsync(result.ErrorReason);
                break;

            case CommandError.BadArgCount:
            case CommandError.ParseFailed:
                await SendUsageAsync(command.Value, context);
                break;

            default:
                await context.Channel.SendMessageAsync($"Something went wrong: {result.ErrorReason}");
                break;
        }
    }

    private async Task SlashCommandExecutedAsync(
        SlashCommandInfo command,
        IInteractionContext context,
        Discord.Interactions.IResult result
    )
    {
        if (!result.IsSuccess)
        {
            await context.Interaction.RespondAsync(
                result.ErrorReason ?? "An unknown error occured",
                ephemeral: true
            );
        }
    }

    private async Task SendUsageAsync(CommandInfo command, ICommandContext context)
    {

        var usage = $"${command.Name}";

        foreach (var param in command.Parameters)
        {
            usage += param.IsOptional ? $" [{param.Name}]" : $"<{param.Name}>";
        }

        await context.Channel.SendMessageAsync($"Usage: {usage}");
    }
}