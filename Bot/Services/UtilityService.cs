namespace SharpCord.Bot.Services;

public class UtilityService()
{
#pragma warning disable CA1822
    public int GetRandomNumber(int maxValue) => Random.Shared.Next(1, maxValue + 1);
#pragma warning restore CA1822
}