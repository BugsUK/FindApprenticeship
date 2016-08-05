namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyStatus
{
    using Common.Mediators;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels.VacancyStatus;
    using VacancyPosting;

    public class VacancyStatusMediator : IVacancyStatusMediator
    {
        private readonly IVacancyStatusChangeProvider _vacancyStatusChangeProvider;

        public VacancyStatusMediator(IVacancyStatusChangeProvider vacancyStatusChangeProvider)
        {
            _vacancyStatusChangeProvider = vacancyStatusChangeProvider;
        }

        public MediatorResponse<ArchiveVacancyViewModel> GetArchiveVacancyViewModelByVacancyReferenceNumber(int vacancyReferenceNumber)
        {
            var model = _vacancyStatusChangeProvider.GetArchiveVacancyViewModelByVacancyReferenceNumber(vacancyReferenceNumber);
            return new MediatorResponse<ArchiveVacancyViewModel>()
            {
                ViewModel = model,
                Code = VacancyStatusMediatorCodes.GetArchiveVacancyViewModel.Ok
            };
        }
    }
}