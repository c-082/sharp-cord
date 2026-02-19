using Discord.Interactions;

using SharpCord.Bot.Services;

namespace SharpCord.Bot.Modules;

public class UtilitySlashModule(UtilityService utility) : InteractionModuleBase<SocketInteractionContext>
{
    private readonly UtilityService _utility = utility;

    [SlashCommand("ping", "Displays the bot's ping.")]
    public async Task PingAsync() => await RespondAsync($"Pong! {Context.Client.Latency}ms");

    [SlashCommand("echo", "Repeats your message.")]
    public async Task EchoAsync([Summary("text", "Text to repeat")] string message) => await RespondAsync(message);

    [SlashCommand("rng", "Produces a random number within a specified range")]
    public async Task RNGAsync(int maxValue)
    {
        if (maxValue <= 0)
        {
            await RespondAsync($"""
            Usage:
            `$rng <maxValue>`
            where maxValue is an integer between 1 and {int.MaxValue}
            """);
            return;
        }

        await RespondAsync($"You got {_utility.GetRandomNumber(maxValue)} in the range 1-{maxValue}!");
    }
}