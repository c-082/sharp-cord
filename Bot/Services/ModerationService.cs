using Discord;
using Discord.WebSocket;

namespace SharpCord.Bot.Services;

public class ModerationService
{
#pragma warning disable CA1822
    public async Task KickAsync(
        SocketGuild guild,
        SocketGuildUser moderator,
        SocketGuildUser target,
        string? reason = null
    )
    {
        if (target.Id == guild.OwnerId)
        {
            throw new InvalidOperationException("Cannot kick the server owner.");
        }

        if (target.Hierarchy >= moderator.Hierarchy)
        {
            throw new InvalidOperationException("You can't kick someone with equal or higher role.");
        }

        if (target.Hierarchy >= guild.CurrentUser.Hierarchy)
        {
            throw new InvalidOperationException("I can't kick someone with equal or higher role than me.");
        }

        await target.KickAsync(reason);
    }

    public async Task BanAsync(
        SocketGuild guild,
        SocketGuildUser moderator,
        SocketGuildUser target,
        int pruneDays,
        string? reason
    )
    {
        if (target.Id == guild.OwnerId)
        {
            throw new InvalidOperationException("Cannot ban the server owner.");
        }

        if (target.Hierarchy >= moderator.Hierarchy)
        {
            throw new InvalidOperationException("You can't ban someone with equal or higher role.");
        }

        if (target.Hierarchy >= guild.CurrentUser.Hierarchy)
        {
            throw new InvalidOperationException("I can't ban someone with equal or higher role than me.");
        }

        Math.Clamp(pruneDays, 0, 7);

        await target.BanAsync(pruneDays, reason);
    }
#pragma warning restore CA1822
}