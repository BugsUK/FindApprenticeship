namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    using System;
    using Common.Mediators;
    using Providers;
    using ViewModels.Vacancy;

    public class VacancyPostingMediator : MediatorBase, IVacancyPostingMediator
    {
        private readonly IVacancyPostingProvider _vacancyPostingProvider;

        public VacancyPostingMediator(IVacancyPostingProvider vacancyPostingProvider)
        {
            _vacancyPostingProvider = vacancyPostingProvider;
        }

        public NewVacancyViewModel Index(string username)
        {
            return _vacancyPostingProvider.GetNewVacancy(username);
        }

        public MediatorResponse<VacancyViewModel> CreateVacancy(NewVacancyViewModel viewModel)
        {
            var vacancyViewModel = new VacancyViewModel
            {
                VacancyReferenceNumber = DateTime.UtcNow.Ticks
            };

            return GetMediatorResponse(VacancyPostingMediatorCodes.CreateVacancy.Ok, vacancyViewModel);
        }

        public MediatorResponse<VacancyViewModel> GetVacancyViewModel(long vacancyReferenceNumber)
        {
            var viewModel = new VacancyViewModel
            {
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetVacancyViewModel.Ok, viewModel);
        }

        public MediatorResponse<VacancyViewModel> SubmitVacancy(VacancyViewModel viewModel)
        {
            return GetMediatorResponse(VacancyPostingMediatorCodes.SubmitVacancy.Ok, viewModel);
        }

        public MediatorResponse<SubmittedVacancyViewModel> GetSubmittedVacancyViewModel(long vacancyReferenceNumber)
        {
            var viewModel = new SubmittedVacancyViewModel
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                ApproverEmail = "john.smith@salon-secrets.co.uk",
                PublishDate = new DateTime(2015, 10, 12)
            };

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetSubmittedVacancyViewModel.Ok, viewModel);
        }
    }
}
