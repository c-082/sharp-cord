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

    [Command("rng")]
    [Summary("Produces a random number within a specified range")]
    public async Task RNGAsync(int maxValue)
    {
        if (maxValue <= 0)
        {
            await ReplyAsync($"""
            Usage:
            `$rng <maxValue>`
            where maxValue is an integer between 1 and {int.MaxValue}
            """);
            return;
        }

        await ReplyAsync($"You got {_utility.GetRandomNumber(maxValue)} in the range 1-{maxValue}!");
    }
}