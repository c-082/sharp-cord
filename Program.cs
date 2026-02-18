using SharpCord.Bot;
using SharpCord.Bot.Services;

var bot = new Bot();
KeepAlive.Start();
await bot.RunAsync();
