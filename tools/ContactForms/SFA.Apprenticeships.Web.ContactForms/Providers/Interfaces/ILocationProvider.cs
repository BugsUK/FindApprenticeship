namespace SFA.Apprenticeships.Web.ContactForms.Providers.Interfaces
{
    using ViewModels;

    public interface ILocationProvider
    {
        LocationsViewModel FindAddress(string postcode); 
    }
}