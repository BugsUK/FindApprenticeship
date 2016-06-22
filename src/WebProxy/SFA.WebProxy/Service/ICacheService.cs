namespace SFA.WebProxy.Service
{
    using System;

    public interface ICacheService
    {
        T Get<T>(string key, Func<T> valueFunc);
    }
}