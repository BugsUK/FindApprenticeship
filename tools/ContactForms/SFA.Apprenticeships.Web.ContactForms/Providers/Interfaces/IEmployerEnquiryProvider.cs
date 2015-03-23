namespace SFA.Apprenticeships.Web.ContactForms.Providers.Interfaces
{
    using Domain.Enums;
    using ViewModels;

    public interface IEmployerEnquiryProvider
    {
        SubmitQueryStatus SubmitEnquiry(EmployerEnquiryViewModel message);
        SubmitQueryStatus SubmitGlaEnquiry(EmployerEnquiryViewModel message);
    }
}