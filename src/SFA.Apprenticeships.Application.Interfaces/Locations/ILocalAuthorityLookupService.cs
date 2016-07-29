namespace SFA.Apprenticeships.Application.Interfaces.Locations
{
    public interface ILocalAuthorityLookupService
    {
        string GetLocalAuthorityCode(string postcode);
    }
}