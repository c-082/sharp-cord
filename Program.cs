using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;

using SharpCord.Bot;
using SharpCord.Bot.Handlers;
using SharpCord.Bot.Services;

var services = new ServiceCollection()
    .AddSingleton<DiscordSocketClient>(provider =>
        new(new()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        }))
    .AddSingleton<CommandService>()
    .AddSingleton<InteractionService>(provider =>
        new(provider.GetRequiredService<DiscordSocketClient>().Rest))
    .AddSingleton<CommandService>()
    .AddSingleton<UtilityService>()
    .AddSingleton<CommandHandler>()
    .AddSingleton<Bot>()
    .BuildServiceProvider();

var bot = services.GetRequiredService<Bot>();
KeepAlive.Start();
await bot.RunAsync();
