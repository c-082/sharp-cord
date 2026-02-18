using Discord.Interactions;

using SharpCord.Bot.Services;

namespace SharpCord.Bot.Modules;

public class UtilitySlashModule(UtilityService utility) : InteractionModuleBase<SocketInteractionContext>
{
    private readonly UtilityService _utility = utility;

    [SlashCommand("ping", "Displays the bot's ping.")]
    public async Task PingAsync() => await ReplyAsync($"Pong! {Context.Client.Latency}ms");

    [SlashCommand("echo", "Repeats your message.")]
    public async Task EchoAsync([Summary("text", "Text to repeat")] string message) => await ReplyAsync(message);
}