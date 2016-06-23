namespace SFA.WebProxy.Repositories
{
    using System;
    using Models;
    using Service;

    public class CachedWebProxyUserRepository : IWebProxyUserRepository
    {
        private readonly IWebProxyUserRepository _webProxyUserRepository;
        private readonly ICacheService _cacheService;

        public CachedWebProxyUserRepository(IWebProxyUserRepository webProxyUserRepository, ICacheService cacheService)
        {
            _webProxyUserRepository = webProxyUserRepository;
            _cacheService = cacheService;
        }

        public WebProxyConsumer Get(Guid externalSystemId)
        {
            return _cacheService.Get($"CachedWebProxyUserRepository_Get_{externalSystemId}", () => _webProxyUserRepository.Get(externalSystemId));
        }
    }
}