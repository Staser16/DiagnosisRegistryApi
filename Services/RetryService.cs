namespace DiagnosisRepositoryApi.Services;

public class RetryService
{
    private const int MaxAttempts = 3;
    private const int DelayMs = 50;

    public async Task<T> ExecuteAsync<T>(Func<int, Task<T>> call, Func<T, bool> shouldRetry)
    {
        var result = await call(1);

        for(var attempt=2; attempt<= MaxAttempts; attempt++)
        {
            if (!shouldRetry(result)) return result;
            await Task.Delay(DelayMs);
            result = await call(attempt);
        }

        return result;
    }

}
