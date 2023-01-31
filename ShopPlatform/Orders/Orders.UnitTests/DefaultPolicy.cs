using Polly;

namespace Orders;

public static class DefaultPolicy
{
    private static readonly Random random = new();
    public static IAsyncPolicy Instance { get; } = Policy.Handle<Exception>().WaitAndRetryAsync(5, CalculateDelay);

    private static TimeSpan CalculateDelay(int retries)
    {
        int delayMilliseconds = 100;
        for (int i = 1; i < retries; i++)
        {
            delayMilliseconds *= 2;
            // 충돌 방지를 위해서
            delayMilliseconds += random.Next(20);
        }
        return TimeSpan.FromMilliseconds(delayMilliseconds);
    }
}