namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Api
{
    using Domain.Entities.Raa.Api;

    public class ApiEndpointViewModel
    {
        public ApiEndpoint Endpoint { get; set; }
        public bool Authorised { get; set; }
    }
}