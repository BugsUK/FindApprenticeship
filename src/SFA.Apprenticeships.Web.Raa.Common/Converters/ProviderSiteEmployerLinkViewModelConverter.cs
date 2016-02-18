namespace SFA.Apprenticeships.Web.Raa.Common.Converters
{
    using Domain.Entities.Raa.Parties;
    using ViewModels.Provider;

    public static class ProviderSiteEmployerLinkViewModelConverter
    {
        public static ProviderSiteEmployerLinkViewModel Convert(this VacancyParty vacancyParty)
        {
            var viewModel = new ProviderSiteEmployerLinkViewModel
            {
                //ProviderSiteEdsErn = vacancyParty.ProviderSiteEdsErn,
                Description = vacancyParty.EmployerDescription,
                WebsiteUrl = vacancyParty.EmployerWebsiteUrl,
                //Employer = vacancyParty.Employer.Convert()
            };

            return viewModel;
        }
    }
}