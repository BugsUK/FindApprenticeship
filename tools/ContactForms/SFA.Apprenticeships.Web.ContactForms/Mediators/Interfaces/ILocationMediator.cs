namespace SFA.Apprenticeships.Web.ContactForms.Mediators.Interfaces
{
    using ViewModels;

    public interface ILocationMediator
    {
        MediatorResponse<LocationsViewModel> FindAddress(string postcode);
    }
}