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

        public MediatorResponse<SubmittedVacancyViewModel> GetSubmittedVacancyViewModel(Guid id)
        {
            var viewModel = new SubmittedVacancyViewModel
            {
                ApproverEmail = "john.smith@salon-secrets.co.uk",
                PublishDate = new DateTime(2015, 10, 12)
            };

            return GetMediatorResponse(VacancyPostingMediatorCodes.GetSubmittedVacancyViewModel.Ok, viewModel);
        }
    }
}
