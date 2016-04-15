﻿namespace SFA.Apprenticeships.Web.Raa.Common.Converters
{
    using Domain.Entities.Raa.Parties;
    using ViewModels.Provider;

    public static class ProviderSiteEmployerLinkViewModelConverter
    {
        public static VacancyPartyViewModel Convert(this VacancyParty vacancyParty, Employer employer)
        {
            var viewModel = new VacancyPartyViewModel
            {
                VacancyPartyId = vacancyParty.VacancyPartyId,
                ProviderSiteId = vacancyParty.ProviderSiteId,
                EmployerDescription = vacancyParty.EmployerDescription,
                EmployerWebsiteUrl = vacancyParty.EmployerWebsiteUrl,
                Employer = employer.Convert()
            };

            return viewModel;
        }
    }
}