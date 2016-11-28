namespace SFA.Apprenticeships.Service.Vacancy
{
    using System;
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
            if (typeof(TSettings).Name == "SearchFactorConfiguration")
            {
                return _searchFactorConfiguration as TSettings;
            }

            return _configurationService.Get<TSettings>();
        }

        public object Get(Type settingsType)
        {
            return _configurationService.Get(settingsType);
        }
    }
}