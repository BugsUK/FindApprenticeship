namespace SFA.Apprenticeships.Web.ContactForms.Mediators.Interfaces
{
    using ViewModels;

    public interface IAccessRequestMediator
    {
       MediatorResponse<AccessRequestViewModel> SubmitAccessRequest();
       MediatorResponse<AccessRequestViewModel> SubmitAccessRequest(AccessRequestViewModel viewModel);
    }
}