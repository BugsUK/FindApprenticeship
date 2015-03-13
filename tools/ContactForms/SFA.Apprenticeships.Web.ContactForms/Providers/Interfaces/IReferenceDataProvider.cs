namespace SFA.Apprenticeships.Web.ContactForms.Providers.Interfaces
{
    using Domain.Enums;
    using ViewModels;

    public interface IReferenceDataProvider
    {
        ReferenceDataListViewModel GetReferenceData(ReferenceDataTypes type);
    }
}