namespace SFA.Apprenticeships.Web.ContactForms.Mediators.Interfaces
{
    using Domain.Enums;
    using ViewModels;

    public interface IReferenceDataMediator
    {
        MediatorResponse<ReferenceDataListViewModel> GetReferenceData(ReferenceDataTypes type);
    }
}