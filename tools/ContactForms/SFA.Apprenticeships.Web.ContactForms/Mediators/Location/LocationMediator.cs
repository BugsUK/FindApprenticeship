namespace SFA.Apprenticeships.Web.ContactForms.Mediators.Location
{
    using Interfaces;
    using Providers.Interfaces;
    using ViewModels;

    public class LocationMediator : MediatorBase, ILocationMediator
    {
        private readonly ILocationProvider _locationProvider;

        public LocationMediator(ILocationProvider locationProvider)
        {
            _locationProvider = locationProvider;
        }

        public MediatorResponse<LocationsViewModel> FindAddress(string postcode)
        {
            var result = _locationProvider.FindAddress(postcode);
            return GetMediatorResponse(LocationMediatorCodes.FindAddress.Success, result);
        }
    }
}