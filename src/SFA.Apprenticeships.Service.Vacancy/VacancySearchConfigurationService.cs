namespace SFA.Apprenticeships.Service.Vacancy
{
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.VacancySearch.Configuration;

    using SFA.Apprenticeships.Application.Interfaces;

    public class VacancySearchConfigurationService : IConfigurationService
    {
        private readonly IConfigurationService _configurationService;
        private readonly SearchFactorConfiguration _searchFactorConfiguration;

        public VacancySearchConfigurationService(
            IConfigurationService configurationService,
            SearchFactorConfiguration searchFactorConfiguration)
        {
            _configurationService = configurationService;
            _searchFactorConfiguration = searchFactorConfiguration;
        }

        public TSettings Get<TSettings>() where TSettings : class
        {
            if (typeof(TSettings).Name == "SearchFactorConfiguration")
            {
                return _searchFactorConfiguration as TSettings;
            }

            return _configurationService.Get<TSettings>();
        }
    }
}