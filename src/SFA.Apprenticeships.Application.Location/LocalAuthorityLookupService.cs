using SFA.Apprenticeships.Application.Interfaces.Locations;

namespace SFA.Apprenticeships.Application.Location
{
    using System;

    [Obsolete]
    public class LocalAuthorityLookupService : ILocalAuthorityLookupService
    {
        private readonly ILocalAuthorityLookupProvider _localAuthorityLookupProvider;

        public LocalAuthorityLookupService(ILocalAuthorityLookupProvider localAuthorityLookupProvider)
        {
            _localAuthorityLookupProvider = localAuthorityLookupProvider;
        }

        public string GetLocalAuthorityCode(string postcode)
        {
            return _localAuthorityLookupProvider.GetLocalAuthorityCode(postcode);
        }
    }
}