using Discord;
using Discord.Commands;
using Discord.WebSocket;

using SharpCord.Bot.Services;

namespace SharpCord.Bot.Modules;

public class ModerationModule(ModerationService moderation) : ModuleBase<SocketCommandContext>
{
    private readonly ModerationService _moderation = moderation;

    [Command("kick")]
    [Summary("Kicks a member.")]
    [RequireContext(ContextType.Guild)]
    [RequireUserPermission(GuildPermission.KickMembers)]
    [RequireBotPermission(GuildPermission.KickMembers)]
    public async Task KickAsync(
        [Summary("Member to kick.")] SocketGuildUser target,
        [Summary("Reason for kicking the member.")][Remainder] string? reason = null)
    {
        var moderator = Context.Guild.GetUser(Context.User.Id);

        try
        {
            await _moderation.KickAsync(Context.Guild,
                moderator,
                target,
                reason);
        }
        catch (InvalidOperationException e)
        {
            await ReplyAsync(e.Message);
            return;
        }

        await ReplyAsync($"{target} kicked by {moderator}. Reason: {reason ?? "N/A"}");
    }

    [Command("ban")]
    [Summary("Bans a member.")]
    [RequireContext(ContextType.Guild)]
    [RequireUserPermission(GuildPermission.BanMembers)]
    [RequireBotPermission(GuildPermission.BanMembers)]
    public async Task BanAsync(
        [Summary("Member to ban.")] SocketGuildUser target,
        [Summary("From past how many days to delete the member's messages.")] int pruneDays = 0,
        [Summary("Reason to ban the member.")][Remainder] string? reason = null)
    {
        var moderator = Context.Guild.GetUser(Context.User.Id);
        try
        {
            await _moderation.BanAsync(Context.Guild,
            moderator,
            target,
            pruneDays,
            reason
            );
        }
        catch (InvalidOperationException e)
        {
            await ReplyAsync(e.Message);
            return;
        }

        await ReplyAsync($"{target} banned by {moderator}. Messages sent by {target} from the past {Math.Clamp(pruneDays, 0, 7)} days deleted. Reason: {reason ?? "N/A"}");
    }
}