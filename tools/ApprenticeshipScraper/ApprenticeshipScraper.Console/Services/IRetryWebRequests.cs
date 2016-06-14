namespace ApprenticeshipScraper.CmdLine.Services
{
    using System;

    public interface IRetryWebRequests
    {
        T RetryWeb<T>(Func<T> action, Action<Exception> onError);
    }
}