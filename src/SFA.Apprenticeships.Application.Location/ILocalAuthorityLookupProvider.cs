namespace SFA.Apprenticeships.Application.Location
{
    public interface ILocalAuthorityLookupProvider
    {
        string GetLocalAuthorityCode(string postcode);
    }
}