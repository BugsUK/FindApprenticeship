namespace SFA.Apprenticeships.Web.Raa.Common.Converters
{
    using Domain.Entities.Raa.Parties;
    using ViewModels.Provider;

    public static class ProviderSiteEmployerLinkViewModelConverter
    {
        public static VacancyOwnerRelationshipViewModel Convert(
            this VacancyOwnerRelationship vacancyOwnerRelationship, Employer employer, string anonymousEmployerName = null)
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
            if (!string.IsNullOrWhiteSpace(anonymousEmployerName))
            {
                viewModel.Employer.FullName = anonymousEmployerName;
            }

            return viewModel;
        }
    }
}