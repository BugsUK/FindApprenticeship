namespace SFA.Apprenticeships.Web.ContactForms.Mediators.ReferenceData
{
    using Domain.Enums;
    using Interfaces;
    using Providers.Interfaces;
    using ViewModels;

    public class ReferenceDataMediator : MediatorBase, IReferenceDataMediator
    {
        private readonly IReferenceDataProvider _referenceDataProvider;

        public ReferenceDataMediator(IReferenceDataProvider referenceDataProvider)
        {
            _referenceDataProvider = referenceDataProvider;
        }

        public MediatorResponse<ReferenceDataListViewModel> GetReferenceData(ReferenceDataTypes type)
        {
            var result = _referenceDataProvider.GetReferenceData(type);
            return GetMediatorResponse(ReferenceDataMediatorCodes.ReferenceData.Success, result);
        }
    }
}