namespace SFA.Apprenticeships.Web.Raa.Common.Converters
{
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using ViewModels.Provider;

    public static class ProviderSiteEmployerLinkViewModelConverter
    {
        public static VacancyOwnerRelationshipViewModel Convert(
            this VacancyOwnerRelationship vacancyOwnerRelationship, Employer employer, Vacancy vacancy = null)
        {
            var viewModel = new VacancyOwnerRelationshipViewModel
            {
                VacancyOwnerRelationshipId = vacancyOwnerRelationship.VacancyOwnerRelationshipId,
                ProviderSiteId = vacancyOwnerRelationship.ProviderSiteId,
                EmployerDescription = vacancyOwnerRelationship.EmployerDescription,
                EmployerWebsiteUrl = vacancyOwnerRelationship.EmployerWebsiteUrl,
                Employer = employer.Convert(),
                IsEmployerAddressValid = true
            };

            if (!string.IsNullOrWhiteSpace(vacancy?.EmployerAnonymousName))
            {
                viewModel.IsAnonymousEmployer = true;
                viewModel.Employer.FullName = vacancy.EmployerAnonymousName;
                viewModel.OriginalFullName = employer.FullName;
                viewModel.AnonymousEmployerReason = vacancy.EmployerAnonymousReason ?? string.Empty;
                viewModel.AnonymousEmployerDescription = vacancy.EmployerAnonymousName;
                viewModel.AnonymousAboutTheEmployer = vacancy.AnonymousAboutTheEmployer;
            }
            return viewModel;
        }
    }
}