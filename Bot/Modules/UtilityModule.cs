using Discord.Commands;

using SharpCord.Bot.Services;

namespace SharpCord.Bot.Modules;

public class UtilityModule(UtilityService utility) : ModuleBase<SocketCommandContext>
{
    private readonly UtilityService _utility = utility;

    [Command("ping")]
    [Summary("Displays the bot's latency.")]
    public async Task PingAsync() => await ReplyAsync($"Pong! {Context.Client.Latency}ms");

    [Command("echo")]
    [Summary("Repeats your message.")]
    public async Task EchoAsync([Remainder] string message) => await ReplyAsync(message);
}