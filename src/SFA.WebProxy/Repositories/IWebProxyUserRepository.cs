namespace SFA.WebProxy.Repositories
{
    using System;
    using Models;

    public interface IWebProxyUserRepository
    {
        WebProxyConsumer Get(Guid externalSystemId);
    }
}