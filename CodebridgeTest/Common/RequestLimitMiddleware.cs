using Common.Configs;

namespace WebAPI.Common;

public class RequestLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly int _limit;
    private readonly TimeSpan _period;
    private readonly Queue<DateTime> _requestTimes;

    public RequestLimitMiddleware(RequestDelegate next, RequestSetting requestSetting)
    {
        _next = next;
        _limit = requestSetting.RequestsAmount;
        _period = TimeSpan.FromSeconds(requestSetting.Period);
        _requestTimes = new Queue<DateTime>();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (ExceedsRateLimit())
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            return;
        }

        EnqueueRequestTime();
        await _next(context);
    }

    private bool ExceedsRateLimit()
    {
        ClearExpiredRequests();
        return _requestTimes.Count >= _limit;
    }

    private void EnqueueRequestTime()
    {
        _requestTimes.Enqueue(DateTime.UtcNow);
    }

    private void ClearExpiredRequests()
    {
        var expiredRequests = new List<DateTime>();

        foreach (var requestTime in _requestTimes)
        {
            if (DateTime.UtcNow - requestTime <= _period)
                break; 
         
            expiredRequests.Add(requestTime);
        }

        foreach (var expiredRequest in expiredRequests)
        {
            _requestTimes.Dequeue();
        }
    }

}
