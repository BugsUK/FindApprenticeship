namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    using System;

    [Obsolete]
    public interface ILocalAuthorityLookupService
    {
        string GetLocalAuthorityCode(string postcode);
    }
}