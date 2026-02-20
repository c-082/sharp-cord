using Discord;
using Discord.WebSocket;
using Discord.Interactions;

using SharpCord.Bot.Services;

namespace SharpCord.Bot.Modules;

public class ModerationSlashModule(ModerationService moderation) : InteractionModuleBase<SocketInteractionContext>
{
    private readonly ModerationService _moderation = moderation;

    [SlashCommand("kick", "Kicks a member.")]
    [RequireContext(ContextType.Guild)]
    [RequireUserPermission(GuildPermission.KickMembers)]
    [RequireBotPermission(GuildPermission.KickMembers)]
    public async Task KickAsync(
        [Summary("target", "Member to kick.")] SocketGuildUser target,
        [Summary("reason", "Reason for kicking the member.")] string? reason = null)
    {
        var moderator = Context.Guild.GetUser(Context.User.Id);
        var botId = Context.Client.CurrentUser.Id;

        try
        {
            await _moderation.KickAsync(Context.Guild,
                moderator,
                target,
                reason,
                botId);
        }
        catch (InvalidOperationException e)
        {
            await RespondAsync(e.Message);
            return;
        }

        await RespondAsync($"{target} kicked by {moderator}.");
    }

    [SlashCommand("ban", "Bans a member.")]
    [RequireContext(ContextType.Guild)]
    [RequireUserPermission(GuildPermission.BanMembers)]
    [RequireBotPermission(GuildPermission.BanMembers)]
    public async Task BanAsync(
        [Summary("target", "Member to ban.")] SocketGuildUser target,
        [Summary("prune-days", "From past how many days to delete the member's messages.")] int pruneDays = 0,
        [Summary("reason", "Reason for banning the user.")] string? reason = null)
    {
        var moderator = Context.Guild.GetUser(Context.User.Id);
        var botId = Context.Client.CurrentUser.Id;

        try
        {
            await _moderation.BanAsync(Context.Guild,
                moderator,
                target,
                pruneDays,
                reason,
                botId);
        }
        catch (InvalidOperationException e)
        {
            await RespondAsync(e.Message);
            return;
        }

        await RespondAsync($"{target} banned by {moderator}. Messages sent by {target} from the past {Math.Clamp(pruneDays, 0, 7)} days deleted. Reason: {reason ?? "N/A"}");
    }
}