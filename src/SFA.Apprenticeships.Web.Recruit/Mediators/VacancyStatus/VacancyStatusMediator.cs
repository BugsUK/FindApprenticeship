namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyStatus
{
    using Common.Mediators;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels.VacancyStatus;
    using VacancyPosting;

    public class VacancyStatusMediator : MediatorBase, IVacancyStatusMediator
    {
        private readonly IVacancyStatusChangeProvider _vacancyStatusChangeProvider;


        public VacancyStatusMediator(IVacancyStatusChangeProvider vacancyStatusChangeProvider)
        {
            _vacancyStatusChangeProvider = vacancyStatusChangeProvider;
        }

        public MediatorResponse<ArchiveVacancyViewModel> GetArchiveVacancyViewModelByVacancyReferenceNumber(int vacancyReferenceNumber)
        {
            var model = _vacancyStatusChangeProvider.GetArchiveVacancyViewModelByVacancyReferenceNumber(vacancyReferenceNumber);
            return new MediatorResponse<ArchiveVacancyViewModel>
            {
                ViewModel = model,
                Code = VacancyStatusMediatorCodes.GetArchiveVacancyViewModel.Ok
            };
        }

        public MediatorResponse<ArchiveVacancyViewModel> ArchiveVacancy(ArchiveVacancyViewModel viewModel)
        {
            var model = _vacancyStatusChangeProvider.ArchiveVacancy(viewModel);

            if (model.HasOutstandingActions)
            {
                return new MediatorResponse<ArchiveVacancyViewModel>()
                {
                    ViewModel = model,
                    Code = VacancyStatusMediatorCodes.ArchiveVacancy.OutstandingActions
                };
            }

            return new MediatorResponse<ArchiveVacancyViewModel>
            {
                ViewModel = model,
                Code = VacancyStatusMediatorCodes.ArchiveVacancy.Ok
            };
        }
    }
}