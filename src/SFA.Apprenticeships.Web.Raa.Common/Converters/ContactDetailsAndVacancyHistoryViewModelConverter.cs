namespace SFA.Apprenticeships.Web.Raa.Common.Converters
{
    using Domain.Entities.Providers;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using ViewModels.Vacancy;

    public static class ContactDetailsAndVacancyHistoryViewModelConverter
    {
        public static ContactDetailsAndVacancyHistoryViewModel Convert(Provider provider, ProviderUser providerUser, Vacancy vacancy)
        {
            if (provider == null || providerUser == null || vacancy == null)
                return null;

            return new ContactDetailsAndVacancyHistoryViewModel
            {
                FullName = providerUser.Fullname,
                ProviderName = provider.Name,
                Email = providerUser.Email,
                PhoneNumber = providerUser.PhoneNumber,
                DateSubmitted = vacancy.DateSubmitted,
                DateFirstSubmitted = vacancy.DateFirstSubmitted ?? vacancy.DateSubmitted,
                DateLastUpdated = vacancy.DateUpdated
            };
        }
    }
}