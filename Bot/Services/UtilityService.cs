namespace SharpCord.Bot.Services;

public class UtilityService()
{
    public int GetRandomNumber(int maxValue) => Random.Shared.Next(1, maxValue + 1);
}