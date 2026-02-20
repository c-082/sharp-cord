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
        string? reason,
        ulong botId
    )
    {
        if (target.Id == guild.OwnerId)
        {
            throw new InvalidOperationException("Cannot kick the server owner.");
        }

        if (target.Id == moderator.Id)
        {
            throw new InvalidOperationException("You can't kick yourself");
        }

        if (target.Id == botId)
        {
            throw new InvalidOperationException("I can't kick myself");
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
        string? reason,
        ulong botId
    )
    {
        if (target.Id == guild.OwnerId)
        {
            throw new InvalidOperationException("Cannot ban the server owner.");
        }

        if (target.Id == moderator.Id)
        {
            throw new InvalidOperationException("You can't ban yourself");
        }

        if (target.Id == botId)
        {
            throw new InvalidOperationException("I can't ban myself");
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