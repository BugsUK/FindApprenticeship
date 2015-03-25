namespace SFA.Apprenticeships.Web.ContactForms.Mediators.Interfaces
{
    using Domain.Enums;
    using ViewModels;

    public interface IEmployerEnquiryMediator
    {
        MediatorResponse<EmployerEnquiryViewModel> SubmitEnquiry(EmployerEnquiryViewModel message);
        MediatorResponse<EmployerEnquiryViewModel> SubmitGlaEnquiry(EmployerEnquiryViewModel message);

        MediatorResponse<EmployerEnquiryViewModel> SubmitEnquiry();
    }
}