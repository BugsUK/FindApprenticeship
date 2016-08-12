namespace SFA.Apprenticeships.Web.Raa.Common.Converters
{
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Users;
    using Domain.Entities.Raa.Vacancies;
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
                ProviderName = provider.TradingName,
                Email = providerUser.Email,
                PhoneNumber = providerUser.PhoneNumber,
                DateSubmitted = vacancy.DateSubmitted,
                DateFirstSubmitted = vacancy.DateFirstSubmitted ?? vacancy.DateSubmitted,
                DateLastUpdated = vacancy.UpdatedDateTime
            };
        }
    }
}