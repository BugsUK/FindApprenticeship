namespace SFA.Apprenticeships.Service.Vacancy
{
    using Infrastructure.VacancySearch.Configuration;
    using Application.Interfaces;

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
            var settingName = typeof(TSettings).Name;
            return Get<TSettings>(settingName);
        }

        public TSettings Get<TSettings>(string settingName) where TSettings : class
        {
            if (settingName == "SearchFactorConfiguration")
            {
                return _searchFactorConfiguration as TSettings;
            }

            return _configurationService.Get<TSettings>(settingName);
        }
    }
}