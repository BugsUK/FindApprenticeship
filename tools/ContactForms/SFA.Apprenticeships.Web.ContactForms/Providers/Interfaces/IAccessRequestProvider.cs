namespace SFA.Apprenticeships.Web.ContactForms.Providers.Interfaces
{
    using ViewModels;

    public interface IAccessRequestProvider
    {
        AccessRequestSubmitStatus SubmitAccessRequest(AccessRequestViewModel viewModel);
    }
}