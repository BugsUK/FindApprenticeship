namespace SFA.Apprenticeships.Web.ContactForms.Providers.Interfaces
{
    using Domain.Enums;
    using ViewModels;

    public interface IEmployerEnquiryProvider
    {
        ReferenceDataListViewModel GetReferenceData(ReferenceDataTypes type);

        SubmitQueryStatus SubmitEnquiry(EmployerEnquiryViewModel message);
    }
}